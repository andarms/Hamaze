using System;
using Hamaze.Arpg.Objects;
using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core.Scenes;
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

        CollisionsManager.Initialize();
        AddLayer("Background", LayerPriority.Background);

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

        // Add more boxes to test collisiongam
        Box box2 = new()
        {
            Position = new Vector2(400, 200)
        };
        AddChild(box2);

        Spikes spikes = new()
        {
            Position = new Vector2(300, 150)
        };
        AddChild(spikes, "Background");


        // TriggerZone zone = new() { Position = new Vector2(500, 100), };
        // Collider collider = new(63, 128) { };
        // zone.AddChild(collider);
        // AddChild(zone);
        // zone.OnCollisionEnter.Connect(_ => player.Health.Heal(10));
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        CollisionsManager.Update(dt);
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
