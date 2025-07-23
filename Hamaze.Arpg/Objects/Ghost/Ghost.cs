using Hamaze.Arpg.Content;
using Hamaze.Arpg.Objects.Ghost.States;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost;

public class Ghost : GameObject
{
    private readonly StateMachine machine;

    public Ghost()
    {
        Sprite sprite = new(AssetsManager.TinyDungeon)
        {
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(16, 160, 16, 16)
        };
        AddChild(sprite);

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