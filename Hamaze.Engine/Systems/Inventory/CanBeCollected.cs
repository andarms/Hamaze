using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CanBeCollected(IItem item) : Trait
{
  public IItem Item { get; } = item;
  public bool AutoCollectionAllowed { get; set; } = true;
}