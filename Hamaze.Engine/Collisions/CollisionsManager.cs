using System;
using System.Collections.Generic;
using System.Linq;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Collisions;

public static class CollisionsManager
{
  const int CELL_SIZE = 64;
  const int WORLD_WIDTH = 10_000;
  const int WORLD_HEIGHT = 10_000;


  public static SpatialGrid Grid { get; private set; } = new SpatialGrid(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);

  private static readonly List<GameObject> objects = [];
  private static readonly Dictionary<GameObject, Rectangle> lastKnownCollisionBounds = [];

  public static void Initialize()
  {
    Grid = new SpatialGrid(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);
    objects.Clear();
    lastKnownCollisionBounds.Clear();
  }

  public static void AddObject(GameObject obj)
  {
    objects.Add(obj);
    lastKnownCollisionBounds[obj] = obj.Bounds;
    Grid.AddObject(obj);
  }

  public static void RemoveObject(GameObject obj)
  {
    if (objects.Remove(obj))
    {
      lastKnownCollisionBounds.Remove(obj);
    }
    Grid.RemoveObject(obj);
  }

  public static void Update(float dt)
  {
    UpdateGrid();
    CheckCollisions();
  }

  /// <summary>
  /// Process collision enter/stay/exit events for all objects using spatial grid
  /// </summary>
  private static void CheckCollisions()
  {

    var checkedPairs = new HashSet<(GameObject, GameObject)>();
    foreach (var objA in objects)
    {
      foreach (var objB in Grid.GetPotentialCollisions(objA))
      {
        var pair = objA.GetHashCode() < objB.GetHashCode() ? (objA, objB) : (objB, objA);
        if (checkedPairs.Contains(pair)) { continue; }
        checkedPairs.Add(pair);

        bool wasColliding = objA.Collisions.Contains(objB);
        bool isColliding = objA.Bounds.Intersects(objB.Bounds);

        TriggerZone? zoneA = objA.Traits.OfType<TriggerZone>().FirstOrDefault();
        TriggerZone? zoneB = objB.Traits.OfType<TriggerZone>().FirstOrDefault();

        if (isColliding && !wasColliding)
        {
          objA.Collisions.Add(objB);
          objB.Collisions.Add(objA);

          zoneA?.OnEnter.Emit(objB);
          zoneB?.OnEnter.Emit(objA);
        }
        else if (!isColliding && wasColliding)
        {
          objA.Collisions.Remove(objB);
          objB.Collisions.Remove(objA);
          zoneA?.OnExit.Emit(objB);
          zoneB?.OnExit.Emit(objA);
        }
      }
    }


  }

  private static void UpdateGrid()
  {
    var objectsToUpdate = new List<(GameObject obj, Rectangle oldBounds)>();
    foreach (var obj in objects)
    {
      if (lastKnownCollisionBounds.TryGetValue(obj, out var lastKnownBounds) && lastKnownBounds != obj.Bounds)
      {
        objectsToUpdate.Add((obj, lastKnownBounds));
      }
    }

    foreach (var (obj, _) in objectsToUpdate)
    {
      Grid.UpdateObject(obj);
      lastKnownCollisionBounds[obj] = obj.Bounds;
    }
  }

  public static IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    foreach (var nearby in Grid.GetPotentialCollisions(obj) ?? [])
    {
      if (nearby == null || obj == nearby) { continue; }
      if (nearby.Collider == null || obj.Collider == null) { continue; }
      if (nearby.Bounds.Intersects(obj.Bounds))
      {
        yield return nearby;
      }
    }
  }

  public static void ResolveSolidCollision(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (other.Traits.Has<Solid>())
    {
      StopObject(obj, other, resolveX, resolveY);
    }
  }

  private static void StopObject(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (obj.Collider == null || other.Collider == null) return;
    if (resolveX)
    {
      if (obj.Position.X < other.Position.X)
      {
        // Object is to the left, push it left
        obj.Position = new Vector2(other.Position.X - obj.Collider.Size.X - obj.Collider.Offset.X, obj.Position.Y);
      }
      else
      {
        // Object is to the right, push it right
        obj.Position = new Vector2(other.Position.X + other.Collider.Size.X - obj.Collider.Offset.X, obj.Position.Y);
      }
    }

    if (resolveY)
    {
      if (obj.Position.Y < other.Position.Y)
      {
        // Object is above, push it up
        obj.Position = new Vector2(obj.Position.X, other.Position.Y - obj.Collider.Size.Y - obj.Collider.Offset.Y);
      }
      else
      {
        // Object is below, push it down
        obj.Position = new Vector2(obj.Position.X, other.Position.Y + other.Collider.Size.Y - obj.Collider.Offset.Y);
      }
    }
  }

  public static void Clear()
  {
    Grid?.Clear();
  }
}