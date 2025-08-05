using System.Xml.Linq;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CanBeCollected : Trait
{
  public Item? Item { get; set; } = null;
  public bool AutoCollectionAllowed { get; set; } = true;

  public override string ToString()
  {
    return $"CanBeCollected: {Item?.Name ?? "No Item"}";
  }

  public override XElement Serialize()
  {
    XElement element = base.Serialize();
    if (Item != null)
    {

      XElement itemElement = Item.Serialize();
      element.Add(new XElement(itemElement));
    }
    return element;
  }
}