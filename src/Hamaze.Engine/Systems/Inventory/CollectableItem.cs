using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CollectableItem : GameObject
{

  public override System.Xml.Linq.XElement? Serialize()
  {
    // Ensure trait exists so it gets serialized
    var trait = this.Trait<CanBeCollected>();
    if (trait == null)
    {
      trait = new CanBeCollected();
      this.AddTrait(trait);
    }
    return base.Serialize();
  }

  public override void Initialize()
  {
    base.Initialize();
    var trait = this.Trait<CanBeCollected>();
    if (trait == null)
    {
      trait = new CanBeCollected();
      this.AddTrait(trait);
    }
    ArgumentNullException.ThrowIfNull(trait.Item, "CanBeCollected trait must have an Item assigned.");
    ArgumentNullException.ThrowIfNull(trait.Item.Sprite, "Item assigned to CanBeCollected trait must have a sprite to be collected.");

    AddChild(trait.Item.Sprite);
  }

  public override void Deserialize(System.Xml.Linq.XElement data)
  {
    base.Deserialize(data);
    // Ensure the CanBeCollected trait exists after load and is wired to this.Item
    var collectTrait = this.Trait<CanBeCollected>();
    if (collectTrait == null)
    {
      collectTrait = new CanBeCollected();
      this.AddTrait(collectTrait);
    }
  }
}