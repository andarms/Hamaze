using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost.States;

public class MoveDown(DynamicObject ghost) : State
{
    public float Speed { get; set; } = 100f;
    public Signal BottomReached = new();

    public override void Update(float dt)
    {
        base.Update(dt);
        ghost.Velocity = new Vector2(0, 1) * Speed;

        if (ghost.Position.Y >= 500)
        {
            BottomReached.Emit();
        }
    }
}