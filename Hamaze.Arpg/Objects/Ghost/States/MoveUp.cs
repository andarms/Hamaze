using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost.States;

public class MoveUp(DynamicObject ghost) : State
{
    public float Speed { get; set; } = 100f;
    public Signal TopReached = new();

    public override void Update(float dt)
    {
        base.Update(dt);
        ghost.Velocity = new Vector2(0, -1) * Speed;

        if (ghost.Position.Y <= 0)
        {
            TopReached.Emit();
        }
    }
}