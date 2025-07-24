using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public record Collision(PhysicsObject ObjectA, PhysicsObject ObjectB, Vector2 Normal, float Penetration)
{
    /// <summary>
    /// Get the other object in the collision from the perspective of the given object
    /// </summary>
    public PhysicsObject GetOther(PhysicsObject me)
    {
        return me == ObjectA ? ObjectB : ObjectA;
    }

    /// <summary>
    /// Get the collision normal from the perspective of the given object
    /// </summary>
    public Vector2 GetNormal(PhysicsObject me)
    {
        return me == ObjectA ? Normal : -Normal;
    }
}


