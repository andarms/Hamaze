using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

/// <summary>
/// Extension methods for easier physics object creation
/// </summary>
public static class PhysicsObjectExtensions
{
    /// <summary>
    /// Sets up a collider for the physics object with the specified size
    /// </summary>
    public static T WithCollider<T>(this T physicsObject, int width, int height, Vector2 offset = default)
        where T : PhysicsObject
    {
        physicsObject.AddChild(new Collider(width, height, offset));
        return physicsObject;
    }

    /// <summary>
    /// Sets up a collider for the physics object with the specified rectangle
    /// </summary>
    public static T WithCollider<T>(this T physicsObject, Rectangle bounds)
        where T : PhysicsObject
    {
        physicsObject.AddChild(new Collider(bounds));
        return physicsObject;
    }

    /// <summary>
    /// Sets up a square collider for the physics object
    /// </summary>
    public static T WithSquareCollider<T>(this T physicsObject, int size, Vector2 offset = default)
        where T : PhysicsObject
    {
        return physicsObject.WithCollider(size, size, offset);
    }
}
