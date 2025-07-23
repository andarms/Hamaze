using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public static class PhysicsWorld
{
    public static readonly List<PhysicsObject> Objects = [];
    private static SpatialGrid? _spatialGrid;
    private static readonly Dictionary<PhysicsObject, Rectangle> _previousBounds = new();

    // Grid configuration
    private static int _cellSize = 64; // Default cell size
    private static Rectangle _worldBounds = new(-10000, -10000, 20000, 20000); // Default world bounds


    public static void Initialize()
    {
        // Initialize spatial grid for better performance
        // Cell size of 128 pixels is good for objects around 16-32 pixel size
        // World bounds cover a reasonable play area
        InitializeSpatialGrid(cellSize: 128, worldBounds: new Rectangle(-2000, -2000, 4000, 4000));
    }

    /// <summary>
    /// Initializes the spatial grid with custom parameters.
    /// </summary>
    /// <param name="cellSize">Size of each grid cell in pixels</param>
    /// <param name="worldBounds">Bounds of the world space</param>
    public static void InitializeSpatialGrid(int cellSize, Rectangle worldBounds)
    {
        _cellSize = cellSize;
        _worldBounds = worldBounds;
        _spatialGrid = new SpatialGrid(cellSize, worldBounds);

        // Re-add existing objects to the new grid
        foreach (var obj in Objects)
        {
            _spatialGrid.AddObject(obj);
            _previousBounds[obj] = obj.Bounds;
        }
    }

    /// <summary>
    /// Gets the current spatial grid. Initializes with default values if not already initialized.
    /// </summary>
    private static SpatialGrid GetSpatialGrid()
    {
        if (_spatialGrid == null)
        {
            InitializeSpatialGrid(_cellSize, _worldBounds);
        }
        return _spatialGrid!;
    }

    public static void AddObject(PhysicsObject obj)
    {
        Objects.Add(obj);
        GetSpatialGrid().AddObject(obj);
        _previousBounds[obj] = obj.Bounds;
    }

    public static void RemoveObject(PhysicsObject obj)
    {
        Objects.Remove(obj);
        GetSpatialGrid().RemoveObject(obj);
        _previousBounds.Remove(obj);
    }

    public static void Clear()
    {
        Objects.Clear();
        GetSpatialGrid().Clear();
        _previousBounds.Clear();
    }

    public static void Update(float dt)
    {
        UpdateSpatialGrid();
        DetectCollisions();
    }

    /// <summary>
    /// Updates the spatial grid for objects that have moved.
    /// </summary>
    private static void UpdateSpatialGrid()
    {
        var grid = GetSpatialGrid();
        var objectsToUpdate = new List<(PhysicsObject obj, Rectangle oldBounds)>();

        // Check which objects have moved
        foreach (var obj in Objects)
        {
            if (_previousBounds.TryGetValue(obj, out var previousBounds) &&
                previousBounds != obj.Bounds)
            {
                objectsToUpdate.Add((obj, previousBounds));
            }
        }

        // Update moved objects in the grid
        foreach (var (obj, oldBounds) in objectsToUpdate)
        {
            grid.UpdateObject(obj, oldBounds);
            _previousBounds[obj] = obj.Bounds;
        }
    }

    private static void DetectCollisions()
    {
        var currentCollisions = new List<Collision>();
        var grid = GetSpatialGrid();
        var checkedPairs = new HashSet<(PhysicsObject, PhysicsObject)>();

        // Use spatial grid for efficient collision detection
        foreach (var objA in Objects)
        {
            var potentialCollisions = grid.GetPotentialCollisions(objA);

            foreach (var objB in potentialCollisions)
            {
                // Avoid duplicate checks and self-collision
                var pair = objA.GetHashCode() < objB.GetHashCode() ? (objA, objB) : (objB, objA);
                if (checkedPairs.Contains(pair))
                    continue;

                checkedPairs.Add(pair);

                if (objA.Bounds.Intersects(objB.Bounds))
                {
                    CalculateNormalAndPenetration(objA, objB, out var normal, out var penetration);
                    var collision = new Collision(objA, objB, normal, penetration);
                    currentCollisions.Add(collision);

                    ProcessCollisionEvents(objA, objB, collision);
                }
            }
        }

        // Handle collision exit events
        ProcessCollisionExitEvents(currentCollisions);
    }

    private static void ProcessCollisionEvents(PhysicsObject objA, PhysicsObject objB, Collision collision)
    {
        // Check if this is a new collision or continuing collision
        var previousCollisionA = objA.PreviousCollisions.FirstOrDefault(c =>
            (c.ObjectA == objA && c.ObjectB == objB) || (c.ObjectA == objB && c.ObjectB == objA));
        var previousCollisionB = objB.PreviousCollisions.FirstOrDefault(c =>
            (c.ObjectA == objA && c.ObjectB == objB) || (c.ObjectA == objB && c.ObjectB == objA));

        if (previousCollisionA == null && previousCollisionB == null)
        {
            // New collision - trigger OnCollisionEnter
            objA.OnCollisionEnter.Emit(collision);
            objB.OnCollisionEnter.Emit(collision);
            objA.PreviousCollisions.Add(collision);
            objB.PreviousCollisions.Add(collision);
        }
        else
        {
            // Continuing collision - trigger OnCollisionStay
            objA.OnCollisionStay.Emit(collision);
            objB.OnCollisionStay.Emit(collision);

            // Update the collision in previous collisions
            if (previousCollisionA != null)
            {
                objA.PreviousCollisions.Remove(previousCollisionA);
                objA.PreviousCollisions.Add(collision);
            }
            if (previousCollisionB != null)
            {
                objB.PreviousCollisions.Remove(previousCollisionB);
                objB.PreviousCollisions.Add(collision);
            }
        }
    }

    private static void ProcessCollisionExitEvents(List<Collision> currentCollisions)
    {
        foreach (var obj in Objects)
        {
            var collisionsToRemove = new List<Collision>();

            foreach (var previousCollision in obj.PreviousCollisions)
            {
                // Check if this previous collision is still happening
                bool stillColliding = currentCollisions.Any(c =>
                    (c.ObjectA == previousCollision.ObjectA && c.ObjectB == previousCollision.ObjectB) ||
                    (c.ObjectA == previousCollision.ObjectB && c.ObjectB == previousCollision.ObjectA));

                if (!stillColliding)
                {
                    // Collision has ended - trigger OnCollisionExit
                    obj.OnCollisionExit.Emit(previousCollision);
                    collisionsToRemove.Add(previousCollision);
                }
            }

            // Remove ended collisions
            foreach (var collision in collisionsToRemove)
            {
                obj.PreviousCollisions.Remove(collision);
            }
        }
    }

    private static void CalculateNormalAndPenetration(PhysicsObject objA, PhysicsObject objB, out Vector2 normal, out float penetration)
    {
        var boundsA = objA.Bounds;
        var boundsB = objB.Bounds;

        // Calculate overlap in both axes
        float overlapX = Math.Min(boundsA.Right, boundsB.Right) - Math.Max(boundsA.Left, boundsB.Left);
        float overlapY = Math.Min(boundsA.Bottom, boundsB.Bottom) - Math.Max(boundsA.Top, boundsB.Top);

        // Determine which axis has the smallest overlap (minimum translation vector)
        if (overlapX < overlapY)
        {
            // Collision is horizontal
            penetration = overlapX;
            if (boundsA.Center.X < boundsB.Center.X)
            {
                normal = new Vector2(-1, 0); // A is to the left of B
            }
            else
            {
                normal = new Vector2(1, 0); // A is to the right of B
            }
        }
        else
        {
            // Collision is vertical
            penetration = overlapY;
            if (boundsA.Center.Y < boundsB.Center.Y)
            {
                normal = new Vector2(0, -1); // A is above B
            }
            else
            {
                normal = new Vector2(0, 1); // A is below B
            }
        }
    }

    /// <summary>
    /// Get all physics objects within a specified area
    /// </summary>
    public static List<PhysicsObject> GetObjectsInArea(Rectangle area)
    {
        return GetSpatialGrid().GetObjectsInArea(area);
    }

    /// <summary>
    /// Get all physics objects of a specific type
    /// </summary>
    public static List<T> GetObjectsOfType<T>() where T : PhysicsObject
    {
        return Objects.OfType<T>().ToList();
    }

    /// <summary>
    /// Check if a point is inside any physics object
    /// </summary>
    public static PhysicsObject? GetObjectAtPoint(Vector2 point)
    {
        return GetSpatialGrid().GetObjectAtPoint(point);
    }

    /// <summary>
    /// Perform a raycast and return the first object hit
    /// </summary>
    public static PhysicsObject? Raycast(Vector2 origin, Vector2 direction, float maxDistance)
    {
        return GetSpatialGrid().Raycast(origin, direction, maxDistance);
    }

    /// <summary>
    /// Gets debugging information about the spatial grid performance.
    /// </summary>
    public static (int TotalCells, int ObjectCount, float AverageObjectsPerCell) GetSpatialGridDebugInfo()
    {
        return GetSpatialGrid().GetDebugInfo();
    }
}