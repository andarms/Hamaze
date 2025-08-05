using System.Collections.Generic;
using Hamaze.Engine.Events;

namespace Hamaze.Engine.Systems.Inventory;

public class Inventory
{
  private readonly List<Item> items = [];

  public Signal<Inventory> OnInventoryChanged { get; } = new();

  public void AddItem(Item item)
  {
    items.Add(item);
    OnInventoryChanged.Emit(this);
  }

  public void RemoveItem(Item item)
  {
    items.Remove(item);
    OnInventoryChanged.Emit(this);
  }

  public IReadOnlyList<Item> GetItems()
  {
    return items.AsReadOnly();
  }
}
