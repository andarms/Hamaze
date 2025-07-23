using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

/// <summary>
/// A spatial grid data structure for efficient collision detection and spatial queries.
/// Divides the world space into uniform cells to reduce the number of collision checks.
/// </summary>
public class SpatialGrid
{
    private readonly Dictionary<Point, HashSet<PhysicsObject>> _grid;
    private readonly int _cellSize;
    private readonly Rectangle _worldBounds;

    /// <summary>
    /// Creates a new spatial grid with the specified cell size and world bounds.
    /// </summary>
    /// <param name="cellSize">The size of each grid cell in pixels</param>
    /// <param name="worldBounds">The bounds of the world space</param>
    public SpatialGrid(int cellSize, Rectangle worldBounds)
    {
        _cellSize = cellSize;
        _worldBounds = worldBounds;
        _grid = new Dictionary<Point, HashSet<PhysicsObject>>();
    }

    /// <summary>
    /// Adds an object to the spatial grid based on its bounds.
    /// </summary>
    public void AddObject(PhysicsObject obj)
    {
        var cells = GetCellsForBounds(obj.Bounds);
        foreach (var cell in cells)
        {
            if (!_grid.ContainsKey(cell))
            {
                _grid[cell] = new HashSet<PhysicsObject>();
            }
            _grid[cell].Add(obj);
        }
    }

    /// <summary>
    /// Removes an object from the spatial grid.
    /// </summary>
    public void RemoveObject(PhysicsObject obj)
    {
        var cells = GetCellsForBounds(obj.Bounds);
        foreach (var cell in cells)
        {
            if (_grid.ContainsKey(cell))
            {
                _grid[cell].Remove(obj);
                if (_grid[cell].Count == 0)
                {
                    _grid.Remove(cell);
                }
            }
        }
    }

    /// <summary>
    /// Updates an object's position in the grid. Call this when an object moves.
    /// </summary>
    public void UpdateObject(PhysicsObject obj, Rectangle oldBounds)
    {
        // Remove from old cells
        var oldCells = GetCellsForBounds(oldBounds);
        foreach (var cell in oldCells)
        {
            if (_grid.ContainsKey(cell))
            {
                _grid[cell].Remove(obj);
                if (_grid[cell].Count == 0)
                {
                    _grid.Remove(cell);
                }
            }
        }

        // Add to new cells
        AddObject(obj);
    }

    /// <summary>
    /// Gets all objects that could potentially collide with the given object.
    /// This significantly reduces the number of collision checks needed.
    /// </summary>
    public HashSet<PhysicsObject> GetPotentialCollisions(PhysicsObject obj)
    {
        var potentialCollisions = new HashSet<PhysicsObject>();
        var cells = GetCellsForBounds(obj.Bounds);

        foreach (var cell in cells)
        {
            if (_grid.ContainsKey(cell))
            {
                foreach (var other in _grid[cell])
                {
                    if (other != obj)
                    {
                        potentialCollisions.Add(other);
                    }
                }
            }
        }

        return potentialCollisions;
    }

    /// <summary>
    /// Gets all objects within a specified area.
    /// </summary>
    public List<PhysicsObject> GetObjectsInArea(Rectangle area)
    {
        var objectsInArea = new HashSet<PhysicsObject>();
        var cells = GetCellsForBounds(area);

        foreach (var cell in cells)
        {
            if (_grid.ContainsKey(cell))
            {
                foreach (var obj in _grid[cell])
                {
                    if (obj.Bounds.Intersects(area))
                    {
                        objectsInArea.Add(obj);
                    }
                }
            }
        }

        return objectsInArea.ToList();
    }

    /// <summary>
    /// Gets the first object at the specified point.
    /// </summary>
    public PhysicsObject? GetObjectAtPoint(Vector2 point)
    {
        var cell = GetCellForPoint(point);
        if (_grid.ContainsKey(cell))
        {
            foreach (var obj in _grid[cell])
            {
                if (obj.Bounds.Contains(new Point((int)point.X, (int)point.Y)))
                {
                    return obj;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Performs a raycast and returns the first object hit.
    /// </summary>
    public PhysicsObject? Raycast(Vector2 origin, Vector2 direction, float maxDistance)
    {
        var normalizedDirection = Vector2.Normalize(direction);
        var endPoint = origin + normalizedDirection * maxDistance;

        // Get all cells that the ray passes through
        var cells = GetCellsForLine(origin, endPoint);
        var checkedObjects = new HashSet<PhysicsObject>();

        foreach (var cell in cells)
        {
            if (_grid.ContainsKey(cell))
            {
                foreach (var obj in _grid[cell])
                {
                    if (!checkedObjects.Contains(obj))
                    {
                        checkedObjects.Add(obj);
                        if (LineIntersectsRectangle(origin, endPoint, obj.Bounds))
                        {
                            return obj;
                        }
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Clears all objects from the grid.
    /// </summary>
    public void Clear()
    {
        _grid.Clear();
    }

    /// <summary>
    /// Gets the grid cell coordinate for a world point.
    /// </summary>
    private Point GetCellForPoint(Vector2 point)
    {
        int x = (int)Math.Floor((point.X - _worldBounds.X) / _cellSize);
        int y = (int)Math.Floor((point.Y - _worldBounds.Y) / _cellSize);
        return new Point(x, y);
    }

    /// <summary>
    /// Gets all grid cells that a rectangle spans across.
    /// </summary>
    private List<Point> GetCellsForBounds(Rectangle bounds)
    {
        var cells = new List<Point>();

        var topLeft = GetCellForPoint(new Vector2(bounds.Left, bounds.Top));
        var bottomRight = GetCellForPoint(new Vector2(bounds.Right - 1, bounds.Bottom - 1));

        for (int x = topLeft.X; x <= bottomRight.X; x++)
        {
            for (int y = topLeft.Y; y <= bottomRight.Y; y++)
            {
                cells.Add(new Point(x, y));
            }
        }

        return cells;
    }

    /// <summary>
    /// Gets all grid cells that a line passes through using a simple approach.
    /// </summary>
    private List<Point> GetCellsForLine(Vector2 start, Vector2 end)
    {
        var cells = new HashSet<Point>();
        var startCell = GetCellForPoint(start);
        var endCell = GetCellForPoint(end);

        // Simple approach: add all cells in the bounding box of the line
        int minX = Math.Min(startCell.X, endCell.X);
        int maxX = Math.Max(startCell.X, endCell.X);
        int minY = Math.Min(startCell.Y, endCell.Y);
        int maxY = Math.Max(startCell.Y, endCell.Y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                cells.Add(new Point(x, y));
            }
        }

        return cells.ToList();
    }

    /// <summary>
    /// Checks if a line intersects with a rectangle.
    /// </summary>
    private static bool LineIntersectsRectangle(Vector2 start, Vector2 end, Rectangle rect)
    {
        // Check if either endpoint is inside the rectangle
        if (rect.Contains(new Point((int)start.X, (int)start.Y)) ||
            rect.Contains(new Point((int)end.X, (int)end.Y)))
            return true;

        // Check intersection with each edge of the rectangle
        var topLeft = new Vector2(rect.Left, rect.Top);
        var topRight = new Vector2(rect.Right, rect.Top);
        var bottomLeft = new Vector2(rect.Left, rect.Bottom);
        var bottomRight = new Vector2(rect.Right, rect.Bottom);

        return LineIntersectsLine(start, end, topLeft, topRight) ||
               LineIntersectsLine(start, end, topRight, bottomRight) ||
               LineIntersectsLine(start, end, bottomRight, bottomLeft) ||
               LineIntersectsLine(start, end, bottomLeft, topLeft);
    }

    /// <summary>
    /// Checks if two line segments intersect.
    /// </summary>
    private static bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        if (Math.Abs(d) < 0.0001f) return false; // Lines are parallel

        var t = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
        var u = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

    /// <summary>
    /// Gets debugging information about the grid state.
    /// </summary>
    public (int TotalCells, int ObjectCount, float AverageObjectsPerCell) GetDebugInfo()
    {
        int totalCells = _grid.Count;
        int totalObjects = _grid.Values.SelectMany(cell => cell).Distinct().Count();
        float averageObjectsPerCell = totalCells > 0 ? (float)_grid.Values.Sum(cell => cell.Count) / totalCells : 0;

        return (totalCells, totalObjects, averageObjectsPerCell);
    }
}
