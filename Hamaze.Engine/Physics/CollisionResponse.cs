using System;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public static class CollisionResponse
{
  private static readonly Vector2 LEFT = new(-1, 0);
  private static readonly Vector2 RIGHT = new(1, 0);
  private static readonly Vector2 UP = new(0, -1);
  private static readonly Vector2 DOWN = new(0, 1);

  /// <summary>
  /// Stops the kinematic objects by resolving penetration, moving them to non-colliding positions.
  /// </summary>
  public static void Stop(Collision collision)
  {
    var objA = collision.ObjectA;
    var objB = collision.ObjectB;
    var normal = collision.Normal;
    var penetration = collision.Penetration;

    // Only move kinematic bodies (like players) away from static objects
    bool aIsKinematic = objA is KinematicBody;
    bool bIsKinematic = objB is KinematicBody;

    if (aIsKinematic && !bIsKinematic)
    {
      // Move object A away from object B
      objA.Position += normal * penetration;
    }
    else if (bIsKinematic && !aIsKinematic)
    {
      // Move object B away from object A (reverse the normal)
      objB.Position -= normal * penetration;
    }
    else if (aIsKinematic && bIsKinematic)
    {
      // Both are kinematic, split the movement
      var halfPenetration = penetration * 0.5f;
      objA.Position += normal * halfPenetration;
      objB.Position -= normal * halfPenetration;
    }
    // If neither is kinematic (both static), do nothing
  }
}
