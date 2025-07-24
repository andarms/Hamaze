using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost.States;

public class MoveDown(GameObject ghost) : State
{
    public float Speed { get; set; } = 100f;
    public Signal BottomReached = new();

    public override void Update(float dt)
    {
        base.Update(dt);
        ghost.Position += new Vector2(0, 1) * Speed * dt;

        if (ghost.Position.Y >= 500)
        {
            BottomReached.Emit();
        }
    }
}