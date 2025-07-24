using System;
using Hamaze.Arpg.Objects;
using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
    readonly Color backgroundColor = Color.CornflowerBlue;
    private bool showDebugInfo = false; // Toggle this to show spatial grid debug info

    Player player;
    Ghost ghost;

    public override void Initialize()
    {

        PhysicsWorld.Initialize();

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


        Box box = new()
        {
            Position = new Vector2(600, 64)
        };
        AddChild(box);

        // Add more boxes to test collision
        Box box2 = new()
        {
            Position = new Vector2(400, 200)
        };
        AddChild(box2);

        Box box3 = new()
        {
            Position = new Vector2(300, 150)
        };
        AddChild(box3);
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        PhysicsWorld.Update(dt);
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
