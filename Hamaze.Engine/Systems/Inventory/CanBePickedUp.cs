using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CanBePickedUp(Item item) : Trait("CanBePickedUp")
{
  public Item Item { get; } = item;
  public bool AutoPickup { get; set; } = true;
}