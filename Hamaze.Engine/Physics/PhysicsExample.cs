using Microsoft.Xna.Framework;
using Hamaze.Engine.Physics;
using System;

namespace Hamaze.Engine.Physics;

/// <summary>
/// Example showing how to use the new physics system with colliders
/// </summary>
public static class PhysicsExample
{
    public static void CreateExampleWorld()
    {
        // Initialize physics world
        PhysicsWorld.Initialize();

        // Create solid objects (walls, platforms) with colliders using extension methods
        var wall = new SolidObject
        {
            Position = new Vector2(100, 100),
            Name = "Wall"
        }.WithCollider(32, 128);

        var platform = new SolidObject
        {
            Position = new Vector2(200, 200),
            Name = "Platform"
        }.WithCollider(128, 32);

        // Create dynamic object (player, enemies) with collider
        var player = new DynamicObject
        {
            Position = new Vector2(50, 50),
            Velocity = new Vector2(100, 0), // Moving right at 100 pixels/second
            Name = "Player"
        }.WithSquareCollider(32);

        // Create trigger zone (pickup, checkpoint) with larger collider
        var checkpoint = new TriggerZone
        {
            Position = new Vector2(300, 150),
            Name = "Checkpoint"
        }.WithSquareCollider(64);

        // Set up collision events
        player.OnCollisionEnter.Connect((collision) =>
        {
            var other = collision.GetOther(player);
            Console.WriteLine($"Player entered collision with {other.Name}");

            if (other is TriggerZone)
            {
                Console.WriteLine("Player hit checkpoint!");
            }
        });

        player.OnCollisionExit.Connect((collision) =>
        {
            var other = collision.GetOther(player);
            Console.WriteLine($"Player exited collision with {other.Name}");
        });

        // Initialize all objects
        wall.Initialize();
        platform.Initialize();
        player.Initialize();
        checkpoint.Initialize();
    }

    public static void UpdateExample(float deltaTime)
    {
        // Update physics world - this handles collision detection and events
        PhysicsWorld.Update(deltaTime);

        // Update all dynamic objects - this handles movement and collision resolution
        foreach (var dynamicObj in PhysicsWorld.DynamicObjects)
        {
            dynamicObj.Update(deltaTime);
        }
    }
}
