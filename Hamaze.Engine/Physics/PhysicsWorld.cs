using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public static class PhysicsWorld
{
    public static readonly List<PhysicsObject> Objects = [];

    public static void AddObject(PhysicsObject obj)
    {
        Objects.Add(obj);
    }

    public static void RemoveObject(PhysicsObject obj)
    {
        Objects.Remove(obj);
    }

    public static void Clear()
    {
        Objects.Clear();
    }

    public static void Update(float dt)
    {
        DetectCollisions();
    }

    private static void DetectCollisions()
    {
        var currentCollisions = new List<Collision>();

        for (int i = 0; i < Objects.Count; i++)
        {
            for (int j = i + 1; j < Objects.Count; j++)
            {
                var objA = Objects[i];
                var objB = Objects[j];

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
        return Objects.Where(obj => obj.Bounds.Intersects(area)).ToList();
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
        return Objects.FirstOrDefault(obj => obj.Bounds.Contains(new Point((int)point.X, (int)point.Y)));
    }

    /// <summary>
    /// Perform a raycast and return the first object hit
    /// </summary>
    public static PhysicsObject? Raycast(Vector2 origin, Vector2 direction, float maxDistance)
    {
        var normalizedDirection = Vector2.Normalize(direction);
        var endPoint = origin + normalizedDirection * maxDistance;

        foreach (var obj in Objects)
        {
            if (LineIntersectsRectangle(origin, endPoint, obj.Bounds))
            {
                return obj;
            }
        }

        return null;
    }

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

    private static bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        var d = (a2.X - a1.X) * (b2.Y - b1.Y) - (a2.Y - a1.Y) * (b2.X - b1.X);
        if (Math.Abs(d) < 0.0001f) return false; // Lines are parallel

        var t = ((b1.X - a1.X) * (b2.Y - b1.Y) - (b1.Y - a1.Y) * (b2.X - b1.X)) / d;
        var u = ((b1.X - a1.X) * (a2.Y - a1.Y) - (b1.Y - a1.Y) * (a2.X - a1.X)) / d;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
}