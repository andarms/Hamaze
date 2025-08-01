using System.Collections.Generic;

namespace Hamaze.Engine.Systems.Inventory;

public class Inventory
{
  private readonly List<IItem> items = [];

  public void AddItem(IItem item)
  {
    items.Add(item);
  }

  public void RemoveItem(IItem item)
  {
    items.Remove(item);
  }

  public IReadOnlyList<IItem> GetItems()
  {
    return items.AsReadOnly();
  }
}
