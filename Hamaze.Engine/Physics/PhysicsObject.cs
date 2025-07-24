using System;
using System.Collections.Generic;
using System.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public class PhysicsObject() : GameObject
{
    public Rectangle Bounds => new((int)GlobalPosition.X, (int)GlobalPosition.Y, Collider.Bounds.Width, Collider.Bounds.Height);
    public Signal<Collision> OnCollisionEnter { get; internal set; } = new();

    public Signal<Collision> OnCollisionStay { get; internal set; } = new();

    public Signal<Collision> OnCollisionExit { get; internal set; } = new();

    public List<Collision> PreviousCollisions { get; } = [];
    public Collider Collider { get; set; } = null!;

    public override void Initialize()
    {
        base.Initialize();
        Collider = Children.OfType<Collider>().FirstOrDefault() ?? throw new InvalidOperationException($"{Name} must have a Collider child.");
        PhysicsWorld.AddObject(this);
    }

    public override void Dispose()
    {
        base.Dispose();
        PhysicsWorld.RemoveObject(this);
    }
}

public class DynamicObject : PhysicsObject
{
    public override void Initialize()
    {
        base.Initialize();

        // Automatically handle collisions with solid objects
        OnCollisionEnter.Connect(collision =>
        {
            CollisionResponse.Stop(collision);
        });
    }
}
public class SolidObject : PhysicsObject { }
public class TriggerZone : PhysicsObject { }