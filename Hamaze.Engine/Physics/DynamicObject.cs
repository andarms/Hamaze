using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public class DynamicObject : PhysicsObject
{
    public Vector2 Velocity = Vector2.Zero;
    public override PhysicsObjectType PhysicsType => PhysicsObjectType.Dynamic;

    public override void Update(float dt)
    {
        base.Update(dt);
        PreviousPosition = Position;

        // Move and check for collisions with solid objects
        Vector2 movement = Velocity * dt;
        MoveAndCollide(movement);
    }

    public void MoveAndCollide(Vector2 movement)
    {
        Position = new Vector2(Position.X + movement.X, Position.Y);
        var collision = PhysicsWorld.GetSolidCollision(this);
        if (collision != null)
        {
            // Resolve X collision
            PhysicsWorld.ResolveCollision(this, collision, true, false);
        }
        Position = new Vector2(Position.X, Position.Y + movement.Y);
        collision = PhysicsWorld.GetSolidCollision(this);
        if (collision != null)
        {
            // Resolve Y collision
            PhysicsWorld.ResolveCollision(this, collision, false, true);
        }
    }
}
