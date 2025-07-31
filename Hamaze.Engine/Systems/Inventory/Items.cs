using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Systems.Inventory;

public abstract class Item
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  // <summary>
  // The sprite used in the game world.
  // </summary>
  public Sprite? Sprite { get; set; } = null;
  public Texture2D? InventorySprite { get; set; } = null;

  public abstract void Use();
}


public class Pickup : GameObject
{
  private readonly Item item;

  public Pickup(Item item)
  {
    this.item = item;
    if (item.Sprite == null)
    {
      throw new ArgumentNullException(nameof(item.Sprite), "Item sprite cannot be null.");
    }
    AddChild(item.Sprite);

    HasInteraction interactable = new();
    interactable.OnInteraction.Connect(AddItemToInventory);
    this.AddTrait(interactable);
  }

  private void AddItemToInventory()
  {

  }

}