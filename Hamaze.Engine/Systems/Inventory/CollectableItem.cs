using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CollectableItem : GameObject
{
  [Save]
  public Item? Item { get; set; }

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Item);

    CanBeCollected trait = this.Trait<CanBeCollected>() ?? throw new InvalidOperationException("CollectableItem must have a CanBeCollected trait.");
    ArgumentNullException.ThrowIfNull(trait.Item, "CanBeCollected trait must have an Item assigned.");
    ArgumentNullException.ThrowIfNull(trait.Item.Sprite, "Item assigned to CanBeCollected trait must have a sprite to be collected.");

    AddChild(trait.Item.Sprite);
  }
}