using Microsoft.Xna.Framework;
using Hamaze.Engine.Core.Scenes;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hamaze.Engine.Systems.Inventory;

public class InventoryScene : Scene
{
  public override Color BackgroundColor => Color.CornflowerBlue;

  Inventory inventory;

  public Vector2 GridSize { get; set; } = new(5, 4);
  public List<InventorySlot> Slots { get; private set; } = [];

  public InventoryScene(Inventory inventory)
  {
    this.inventory = inventory;
    Vector2 offset = new(24);
    int padding = 10;
    int slotSize = 24 * Renderer.ScaleFactor;

    Vector2 windowSize = new(
      slotSize * GridSize.X + padding * 2,
      slotSize * GridSize.Y + padding * 2
    );

    Vector2 winidowPosition = new(
      (Renderer.WindowWidth - windowSize.X) / 2,
      (Renderer.WindowHeight - windowSize.Y) / 2
    );

    NinePatchSprite inventoryWindow = new(AssetsManager.Textures["UI/Inventory/Window"], 7)
    {
      Position = winidowPosition,
      Size = windowSize + offset,
    };
    AddChild(inventoryWindow, UIOverlayLayer);

    for (int i = 0; i < GridSize.X; i++)
    {
      for (int j = 0; j < GridSize.Y; j++)
      {
        int x = i * slotSize + padding;
        int y = j * slotSize + padding;
        InventorySlot slot = new()
        {
          Position = new Vector2(x, y) + offset,
        };
        inventoryWindow.AddChild(slot);
        Slots.Add(slot);
      }
    }

    var items = inventory.GetItems();
    for (int i = 0; i < Slots.Count; i++)
    {
      if (i < items.Count)
      {
        Slots[i].AddItem(items[i]);
      }
      else
      {
        Slots[i].Clear();
      }
    }


  }

  public override void Initialize()
  {
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.IsActionJustPressed("ToggleInventory"))
    {
      SceneManager.PopScene();
    }
  }

  public override void Draw(Renderer renderer)
  {
    base.Draw(renderer);
  }


  public void UpdateInventory(Inventory newInventory)
  {
    inventory = newInventory;
    foreach (var slot in Slots)
    {
      slot.Clear();
    }
    var items = inventory.GetItems();
    for (int i = 0; i < items.Count; i++)
    {
      if (i < Slots.Count)
      {
        Slots[i].AddItem(items[i]);
      }
    }
  }
}