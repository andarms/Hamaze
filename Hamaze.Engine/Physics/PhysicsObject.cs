using System;
using System.Collections.Generic;
using System.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public enum PhysicsObjectType
{
    Solid,    // Static solid objects that block movement
    Dynamic,  // Objects that can move and be blocked by solids
    Trigger   // Objects that detect collisions but don't block movement
}

public abstract class PhysicsObject : GameObject
{
    private Collider? collider;

    public Collider Collider
    {
        get => collider ?? throw new InvalidOperationException($"{Name} must have a Collider component.");
        set => collider = value;
    }

    public Rectangle Bounds => new(
        (int)GlobalPosition.X + Collider.Bounds.X,
        (int)GlobalPosition.Y + Collider.Bounds.Y,
        Collider.Bounds.Width,
        Collider.Bounds.Height
    );

    public abstract PhysicsObjectType PhysicsType { get; }

    // Collision events
    public Signal<Collision> OnCollisionEnter { get; } = new();
    public Signal<Collision> OnCollisionStay { get; } = new();
    public Signal<Collision> OnCollisionExit { get; } = new();

    // Internal tracking
    internal HashSet<PhysicsObject> CollidingWith = [];
    internal Vector2 PreviousPosition;

    public override void Initialize()
    {
        base.Initialize();

        // Find collider in children or create a default one
        collider = Children.OfType<Collider>().FirstOrDefault();
        if (collider == null)
        {
            // Create a default collider if none is found
            collider = new Collider(32, 32);
            AddChild(collider);
        }

        PreviousPosition = Position;
        PhysicsWorld.AddObject(this);
    }
    public override void Dispose()
    {
        base.Dispose();
        PhysicsWorld.RemoveObject(this);
    }

    public bool Overlaps(PhysicsObject other)
    {
        return Bounds.Intersects(other.Bounds);
    }
}
