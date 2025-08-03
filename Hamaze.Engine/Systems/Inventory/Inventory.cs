using System.Collections.Generic;
using Hamaze.Engine.Events;

namespace Hamaze.Engine.Systems.Inventory;

public class Inventory
{
  private readonly List<IItem> items = [];

  public Signal<Inventory> OnInventoryChanged { get; } = new();

  public void AddItem(IItem item)
  {
    items.Add(item);
    OnInventoryChanged.Emit(this);
  }

  public void RemoveItem(IItem item)
  {
    items.Remove(item);
    OnInventoryChanged.Emit(this);
  }

  public IReadOnlyList<IItem> GetItems()
  {
    return items.AsReadOnly();
  }
}
