using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
    readonly Color backgroundColor = Color.CornflowerBlue;

    Player player;

    public override void Initialize()
    {
        player = new Player();
        player = new Player
        {
            Position = new Vector2(64, 64)
        };
        AddChild(player);
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
