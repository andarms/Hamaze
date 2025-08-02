using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CollectableItem : GameObject
{
  public IItem Item { get; }

  public CollectableItem(IItem item)
  {
    if (item.Sprite == null)
    {
      throw new ArgumentException("Item must have a sprite to be picked up.", nameof(item));
    }
    Item = item;
    AddChild(item.Sprite);
    this.AddTrait(new CanBeCollected(item));

    CollisionsManager.AddObject(this);
  }
}