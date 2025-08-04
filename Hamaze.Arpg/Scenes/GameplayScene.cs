using System;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Hamaze.Arpg.Objects;
using Hamaze.Arpg.Objects.Ghost;
using Hamaze.Arpg.Objects.Items;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Scenes;
using Hamaze.Engine.Data;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Input;
using Hamaze.Engine.Systems.Inventory;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Scenes;

public class GameplayScene : Scene
{
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


        Box box = new()
        {
            Position = new Vector2(600, 64)
        };
        AddChild(box);


        Chest chest = new()
        {
            Position = new Vector2(400, 200)
        };
        AddChild(chest);

        Spikes spikes = new()
        {
            Position = new Vector2(300, 150)
        };
        AddChild(spikes, "Background");

        CollectableItem sword = new(new SwordItem())
        {
            Position = new Vector2(200, 200),
            Collider = new Collider(
                offset: new Vector2(0, 0),
                size: new Vector2(64)
            )
        };
        AddChild(sword, "Background");

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

        string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "player.xml");
        LoadGameObjects(dataPath);
    }

    private void LoadGameObjects(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Console.WriteLine($"Warning: Could not find player.xml file at {dataPath}");
            return;
        }

        try
        {
            using StreamReader reader = new(dataPath);
            XElement root = XElement.Load(reader);
            var validationResult = XmlSchemaValidator.ValidateGameObject(root);
            validationResult.PrintToConsole();

            if (!validationResult.IsValid)
            {
                Console.WriteLine("XML validation failed. Attempting to load anyway...");
            }

            GameObject gameObject = new();
            gameObject.Deserialize(root);
            if (gameObject != null)
            {
                AddChild(gameObject);
                Console.WriteLine($"Successfully loaded GameObject '{gameObject.Name}' from {dataPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading XML file {dataPath}: {ex.Message}");
        }
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
