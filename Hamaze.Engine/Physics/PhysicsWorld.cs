using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public static class PhysicsWorld
{
    private static readonly List<PhysicsObject> objects = [];
    private static readonly List<SolidObject> solidObjects = [];
    private static readonly List<DynamicObject> dynamicObjects = [];
    private static readonly List<TriggerZone> triggerZones = [];

    // Spatial grid for efficient collision detection
    private static SpatialGrid? spatialGrid;
    private static readonly Dictionary<PhysicsObject, Rectangle> previousBounds = new();

    // Default spatial grid settings
    private static int cellSize = 128;
    private static Rectangle worldBounds = new(-2000, -2000, 4000, 4000);

    public static IReadOnlyList<PhysicsObject> Objects => objects;
    public static IReadOnlyList<SolidObject> SolidObjects => solidObjects;
    public static IReadOnlyList<DynamicObject> DynamicObjects => dynamicObjects;
    public static IReadOnlyList<TriggerZone> TriggerZones => triggerZones;

    public static void Initialize()
    {
        // Initialize spatial grid for better performance
        InitializeSpatialGrid(cellSize, worldBounds);
    }

    /// <summary>
    /// Initializes the spatial grid with custom parameters.
    /// </summary>
    /// <param name="cellSize">Size of each grid cell in pixels</param>
    /// <param name="worldBounds">Bounds of the world space</param>
    public static void InitializeSpatialGrid(int cellSize, Rectangle worldBounds)
    {
        PhysicsWorld.cellSize = cellSize;
        PhysicsWorld.worldBounds = worldBounds;
        spatialGrid = new SpatialGrid(cellSize, worldBounds);

        // Re-add existing objects to the new grid
        foreach (var obj in objects)
        {
            spatialGrid.AddObject(obj);
            previousBounds[obj] = obj.Bounds;
        }
    }

    /// <summary>
    /// Gets the current spatial grid. Initializes with default values if not already initialized.
    /// </summary>
    private static SpatialGrid GetSpatialGrid()
    {
        if (spatialGrid == null)
        {
            InitializeSpatialGrid(cellSize, worldBounds);
        }
        return spatialGrid!;
    }

    #region Object Management
    public static void AddObject(PhysicsObject obj)
    {
        objects.Add(obj);
        GetSpatialGrid().AddObject(obj);
        previousBounds[obj] = obj.Bounds;
        switch (obj.PhysicsType)
        {
            case PhysicsObjectType.Solid:
                solidObjects.Add((SolidObject)obj);
                break;
            case PhysicsObjectType.Dynamic:
                dynamicObjects.Add((DynamicObject)obj);
                break;
            case PhysicsObjectType.Trigger:
                triggerZones.Add((TriggerZone)obj);
                break;
        }
    }

    public static void RemoveObject(PhysicsObject obj)
    {
        objects.Remove(obj);
        GetSpatialGrid().RemoveObject(obj);
        previousBounds.Remove(obj);

        switch (obj.PhysicsType)
        {
            case PhysicsObjectType.Solid:
                solidObjects.Remove((SolidObject)obj);
                break;
            case PhysicsObjectType.Dynamic:
                dynamicObjects.Remove((DynamicObject)obj);
                break;
            case PhysicsObjectType.Trigger:
                triggerZones.Remove((TriggerZone)obj);
                break;
        }
    }

    public static void Clear()
    {
        objects.Clear();
        solidObjects.Clear();
        dynamicObjects.Clear();
        triggerZones.Clear();
        GetSpatialGrid().Clear();
        previousBounds.Clear();
    }
    #endregion

    public static void Update(float dt)
    {
        UpdateSpatialGrid();
        ProcessCollisionEvents();
    }

    /// <summary>
    /// Updates the spatial grid for objects that have moved.
    /// </summary>
    private static void UpdateSpatialGrid()
    {
        var grid = GetSpatialGrid();
        var objectsToUpdate = new List<(PhysicsObject obj, Rectangle oldBounds)>();

        foreach (var obj in objects)
        {
            if (previousBounds.TryGetValue(obj, out var lastKnownBounds) && lastKnownBounds != obj.Bounds)
            {
                objectsToUpdate.Add((obj, lastKnownBounds));
            }
        }

        foreach (var (obj, oldBounds) in objectsToUpdate)
        {
            grid.UpdateObject(obj, oldBounds);
            previousBounds[obj] = obj.Bounds;
        }
    }

    /// <summary>
    /// Get the first solid object that this object is colliding with using spatial grid
    /// </summary>
    public static SolidObject? GetSolidCollision(PhysicsObject obj)
    {
        var grid = GetSpatialGrid();
        var potentialCollisions = grid.GetPotentialCollisions(obj);

        foreach (var other in potentialCollisions)
        {
            if (other is SolidObject solid && solid != obj && obj.Overlaps(solid))
            {
                return solid;
            }
        }
        return null;
    }

    /// <summary>
    /// Resolve collision by moving the object out of the solid
    /// </summary>
    public static void ResolveCollision(PhysicsObject obj, SolidObject solid, bool resolveX, bool resolveY)
    {
        if (resolveX)
        {
            // Move object out along X axis
            if (obj.Position.X < solid.Position.X)
            {
                // Object is to the left, push it left
                obj.Position = new Vector2(solid.Position.X - obj.Collider.Width - obj.Collider.Offset.X, obj.Position.Y);
            }
            else
            {
                // Object is to the right, push it right
                obj.Position = new Vector2(solid.Position.X + solid.Collider.Width - obj.Collider.Offset.X, obj.Position.Y);
            }
        }

        if (resolveY)
        {
            // Move object out along Y axis
            if (obj.Position.Y < solid.Position.Y)
            {
                // Object is above, push it up
                obj.Position = new Vector2(obj.Position.X, solid.Position.Y - obj.Collider.Height - obj.Collider.Offset.Y);
            }
            else
            {
                // Object is below, push it down
                obj.Position = new Vector2(obj.Position.X, solid.Position.Y + solid.Collider.Height - obj.Collider.Offset.Y);
            }
        }
    }

    /// <summary>
    /// Process collision enter/stay/exit events for all objects using spatial grid
    /// </summary>
    private static void ProcessCollisionEvents()
    {
        var grid = GetSpatialGrid();
        var checkedPairs = new HashSet<(PhysicsObject, PhysicsObject)>();

        foreach (var objA in objects)
        {
            var potentialCollisions = grid.GetPotentialCollisions(objA);
            foreach (var objB in potentialCollisions)
            {
                // Create a consistent pair ordering to avoid duplicate checks
                var pair = objA.GetHashCode() < objB.GetHashCode() ? (objA, objB) : (objB, objA);
                if (checkedPairs.Contains(pair)) { continue; }

                checkedPairs.Add(pair);

                // Skip if both are solid objects (they don't need collision events)
                if (objA.PhysicsType == PhysicsObjectType.Solid && objB.PhysicsType == PhysicsObjectType.Solid)
                {
                    continue;
                }

                bool wasColliding = objA.CollidingWith.Contains(objB);
                bool isColliding = objA.Overlaps(objB);

                if (isColliding && !wasColliding)
                {
                    // Collision started
                    var collision = CreateCollision(objA, objB);
                    objA.CollidingWith.Add(objB);
                    objB.CollidingWith.Add(objA);

                    objA.OnCollisionEnter.Emit(collision);
                    objB.OnCollisionEnter.Emit(collision);
                }
                else if (isColliding && wasColliding)
                {
                    // Collision continuing
                    var collision = CreateCollision(objA, objB);
                    objA.OnCollisionStay.Emit(collision);
                    objB.OnCollisionStay.Emit(collision);
                }
                else if (!isColliding && wasColliding)
                {
                    // Collision ended
                    var collision = CreateCollision(objA, objB);
                    objA.CollidingWith.Remove(objB);
                    objB.CollidingWith.Remove(objA);

                    objA.OnCollisionExit.Emit(collision);
                    objB.OnCollisionExit.Emit(collision);
                }
            }
        }
    }

    /// <summary>
    /// Create a collision object with proper normal and penetration
    /// </summary>
    private static Collision CreateCollision(PhysicsObject objA, PhysicsObject objB)
    {
        var boundsA = objA.Bounds;
        var boundsB = objB.Bounds;

        // Calculate overlap
        float overlapX = Math.Min(boundsA.Right, boundsB.Right) - Math.Max(boundsA.Left, boundsB.Left);
        float overlapY = Math.Min(boundsA.Bottom, boundsB.Bottom) - Math.Max(boundsA.Top, boundsB.Top);

        Vector2 normal;
        float penetration;

        // Use the smaller overlap to determine collision normal
        if (overlapX < overlapY)
        {
            penetration = overlapX;
            normal = boundsA.Center.X < boundsB.Center.X ? new Vector2(-1, 0) : new Vector2(1, 0);
        }
        else
        {
            penetration = overlapY;
            normal = boundsA.Center.Y < boundsB.Center.Y ? new Vector2(0, -1) : new Vector2(0, 1);
        }

        return new Collision(objA, objB, normal, penetration);
    }

    /// <summary>
    /// Get all objects within a specified area using spatial grid
    /// </summary>
    public static List<PhysicsObject> GetObjectsInArea(Rectangle area)
    {
        return GetSpatialGrid().GetObjectsInArea(area);
    }


    /// <summary>
    /// Gets debugging information about the spatial grid performance.
    /// </summary>
    public static (int TotalCells, int ObjectCount, float AverageObjectsPerCell) GetSpatialGridDebugInfo()
    {
        return GetSpatialGrid().GetDebugInfo();
    }
}