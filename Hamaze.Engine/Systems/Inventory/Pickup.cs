using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class Pickup : GameObject
{
  public Item Item { get; }

  public Pickup(Item item)
  {
    if (item.Sprite == null)
    {
      throw new ArgumentException("Item must have a sprite to be picked up.", nameof(item));
    }
    Item = item;
    AddChild(item.Sprite);
    this.AddTrait(new CanBePickedUp(item));
  }
}