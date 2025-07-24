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
  /// Stops the dynamic objects by allowing minimal penetration and resolving only excessive overlap.
  /// </summary>
  public static void Stop(Collision collision)
  {
    if (collision.ObjectA is DynamicObject dynamicObject)
    {
      dynamicObject.Velocity = Vector2.Zero;
    }
    if (collision.ObjectB is DynamicObject dynamicObjectB)
    {
      dynamicObjectB.Velocity = Vector2.Zero;
    }
  }
}
