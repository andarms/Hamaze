using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class HasInventory(Inventory inventory) : Trait("HasInventory")
{
  public Inventory Inventory { get; } = inventory;
}