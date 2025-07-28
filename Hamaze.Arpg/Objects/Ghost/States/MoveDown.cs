using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost.States;

public class MoveDown(DynamicObject ghost) : State
{
    public float Speed { get; set; } = 100f;
    public Signal BottomReached = new();

    public override void Update(float dt)
    {
        base.Update(dt);
        var velocity = new Vector2(0, Speed * dt);
        ghost.Move(velocity);

        if (ghost.Position.Y >= 500)
        {
            BottomReached.Emit();
        }
    }
}