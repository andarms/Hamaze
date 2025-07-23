using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public static class CollisionResponse
{
  public static void Push(Collision collision, Vector2 force)
  {
    if (collision.ObjectB is KinematicBody kinematicObject)
    {
      kinematicObject.Position += force * collision.Normal;
    }
  }

  public static void Stop(Collision collision)
  {
    if (collision.ObjectB is KinematicBody kinematicObject)
    {
      kinematicObject.Position = collision.ObjectB.Position - collision.Normal * collision.Penetration;
    }
  }
}