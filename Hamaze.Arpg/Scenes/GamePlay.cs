using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
    readonly Color backgroundColor = Color.CornflowerBlue;

    Player player;
    Ghost ghost;

    public override void Initialize()
    {
        player = new Player
        {
            Position = new Vector2(64, 64)
        };
        AddChild(player);

        ghost = new Ghost
        {
            Position = new Vector2(128, 64)
        };
        AddChild(ghost);
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
