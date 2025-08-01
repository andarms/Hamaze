using System.Collections.Generic;

namespace Hamaze.Engine.Systems.Inventory;

public class Inventory
{
  private readonly List<Item> items = [];

  public void AddItem(Item item)
  {
    items.Add(item);
  }

  public void RemoveItem(Item item)
  {
    items.Remove(item);
  }

  public IReadOnlyList<Item> GetItems()
  {
    return items.AsReadOnly();
  }
}
