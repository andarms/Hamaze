using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Systems.Inventory;

public class InventorySlot : GameObject
{
  public bool Ocupied { get; private set; } = true;
  Rectangle emptySource = new(0, 0, 20, 20);
  Rectangle ocupiedSource = new(0, 20, 20, 20);
  readonly Sprite sprite;

  public IItem? Item { get; private set; }

  public InventorySlot()
  {
    sprite = new()
    {
      Texture = AssetsManager.Textures["UI/Inventory/Slot"],
      Position = Vector2.Zero,
      Color = Color.White,
      Source = emptySource
    };
    AddChild(sprite);
  }

  public void AddItem(IItem item)
  {
    Item = item;
    Ocupied = true;
    sprite.Source = ocupiedSource;
    if (item.InventorySprite == null)
    {
      throw new ArgumentNullException(nameof(item.InventorySprite), "Item sprite cannot be null.");
    }
    item.InventorySprite.Position = new(8);
    AddChild(item.InventorySprite);
  }

  public void Clear()
  {
    Ocupied = false;
    sprite.Source = emptySource;
    Item = null;
  }
}