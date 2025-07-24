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
    private bool _showDebugInfo = false; // Toggle this to show spatial grid debug info

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
            Position = new Vector2(200, 64)
        };
        player.OnCollisionEnter.Connect(collision =>
        {
            CollisionResponse.Stop(collision);
        });
        AddChild(box);
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        PhysicsWorld.Update(dt);

        // Optional: Print spatial grid debug info periodically
        if (_showDebugInfo)
        {
            var debugInfo = PhysicsWorld.GetSpatialGridDebugInfo();
            Console.WriteLine($"Spatial Grid - Cells: {debugInfo.TotalCells}, Objects: {debugInfo.ObjectCount}, Avg/Cell: {debugInfo.AverageObjectsPerCell:F2}");
        }
    }

    public override void Draw(Renderer renderer)
    {
        renderer.ClearBackground(backgroundColor);
        renderer.Begin();
        base.Draw(renderer);
        renderer.End();
    }

}
