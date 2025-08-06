using Hamaze.Arpg.Objects.Ghost.States;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost;

public class Ghost : DynamicObject
{
    private readonly StateMachine machine;

    public Ghost()
    {
        Name = "Ghost";
        Sprite sprite = new()
        {
            Texture = AssetsManager.Textures["Sprites/TinyDungeon"],
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(16, 160, 16, 16)
        };
        AddChild(sprite);

        Collider = new Collider(
            offset: new Vector2(0, 0),
            size: new Vector2(16 * Renderer.ScaleFactor)
        );

        machine = new();
        MoveDown down = new(this);
        down.BottomReached.Connect(() => machine.ChangeState<MoveUp>());
        machine.AddChild(down);

        MoveUp up = new(this);
        up.TopReached.Connect(() => machine.ChangeState<MoveDown>());
        machine.AddChild(up);

        machine.SetInitialState<MoveDown>();
        AddChild(machine);

        this.AddTrait(new IsSolid());
    }

    public override void Initialize()
    {
        base.Initialize();
    }
}