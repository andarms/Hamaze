using System;
using Hamaze.Arpg.Objects;
using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Items;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Scenes;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Input;
using Hamaze.Engine.Systems.Inventory;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
    const string GameDirectory = @"C:\Users\andar\apps\Hamaze\data";

    public override Color BackgroundColor => Color.CornflowerBlue;

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


        Chest chest = new()
        {
            Position = new Vector2(400, 200)
        };
        AddChild(chest);


        Sing sing = new("Hello, world!")
        {
            Position = new Vector2(400, 400)
        };
        AddChild(sing);

        HealingZone healingZone = new() { Position = new Vector2(500, 100), };
        AddChild(healingZone);

        InventoryScene inventoryScene = new(player.Inventory)
        {
            GridSize = new Vector2(5, 4)
        };
        SceneManager.AddScene(inventoryScene);

        player.Inventory.OnInventoryChanged.Connect(inventoryScene.UpdateInventory);


        foreach (GameObject obj in SceneDataLoader.LoadGameObjects(GameDirectory))
        {
            AddChild(obj);
        }

        SceneDataLoader.LoadResources(GameDirectory);


        GameObject spikes = new()
        {
            Name = "Spikes",
            Position = new Vector2(300, 200),

        };
        Sprite sprite = new()
        {
            Position = new Vector2(300, 200),
            Texture = AssetsManager.Textures["Sprites/TinyDungeon"],
            Source = new Rectangle(80, 48, 16, 16),
            Origin = new Vector2(8, 16)
        };
        Hitbox hitbox = new()
        {
            Collider = new Collider
            {
                Size = new Vector2(64, 64),
                Offset = new Vector2(0, 0)
            },
            DamageCalculator = new SimpleDamage(10)
        };
        spikes.AddChild(sprite);
        spikes.AddChild(hitbox);
        Console.WriteLine(spikes.Serialize());
    }


    public override void Update(float dt)
    {
        base.Update(dt);
        CollisionsManager.Update(dt);

        if (InputManager.IsActionJustPressed("ToggleInventory"))
        {
            SceneManager.PushScene<InventoryScene>();
        }
    }

    public override void Draw(Renderer renderer)
    {
        base.Draw(renderer);
    }

}
