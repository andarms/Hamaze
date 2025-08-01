using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CanBePickedUp(IItem item) : Trait("CanBePickedUp")
{
  public IItem Item { get; } = item;
  public bool AutoPickup { get; set; } = true;
}