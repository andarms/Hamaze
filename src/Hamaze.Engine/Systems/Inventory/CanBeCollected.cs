using System.Xml.Linq;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Systems.Inventory;

public class CanBeCollected : Trait
{
  public Item? Item { get; set; } = null;
  public bool AutoCollectionAllowed { get; set; } = true;

  public override XElement Serialize()
  {
    var element = base.Serialize();
    if (Item != null)
    {
      // Serialize the Item as a nested Resource inside the trait
      element.Add(Item.Serialize());
    }
    return element;
  }

  public override void Deserialize(XElement element)
  {
    base.Deserialize(element);
    var itemElement = element.Element("Resource");
    if (itemElement != null)
    {
      var item = new Item();
      item.Deserialize(itemElement);
      Item = item;
    }
  }

  public override string ToString()
  {
    return $"CanBeCollected: {Item?.Name ?? "No Item"}";
  }
}