using Hamaze.Arpg.Content;
using Hamaze.Arpg.Objects.Ghost.States;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost;

public class Ghost : KinematicBody
{
    private readonly StateMachine machine;

    public Ghost()
    {
        Name = "Ghost";
        Sprite sprite = new(AssetsManager.TinyDungeon)
        {
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(16, 160, 16, 16)
        };
        AddChild(sprite);

        Vector2 size = new Vector2(16, 16) * Renderer.ScaleFactor;
        Collider collider = new(new(0, 0, (int)size.X, (int)size.Y));
        AddChild(collider);

        machine = new();
        MoveDown down = new(this);
        down.BottomReached.Connect(() => machine.ChangeState<MoveUp>());
        machine.AddChild(down);

        MoveUp up = new(this);
        up.TopReached.Connect(() => machine.ChangeState<MoveDown>());
        machine.AddChild(up);

        machine.SetInitialState<MoveDown>();
        AddChild(machine);
        // WobbleMovementAnimation wobbleAnimation = new(sprite, movement);
        // AddChild(wobbleAnimation);
    }

    public override void Initialize()
    {
        base.Initialize();
    }
}