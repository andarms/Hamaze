using System;
using System.Xml.Linq;
using Hamaze.Engine.Systems.Inventory;

namespace Hamaze.Engine.Systems.Traits;

public static class TraitFactory
{
  public static Trait? CreateFromType(XElement element)
  {
    string? type = element.Attribute("Type")?.Value;
    if (string.IsNullOrEmpty(type))
    {
      return null;
    }
    return type switch
    {
      "IsSolid" => new IsSolid(),
      "CanBeCollected" => CreateCanBeCollected(element),
      _ => null
    };
  }

  private static CanBeCollected CreateCanBeCollected(XElement element)
  {
    CanBeCollected trait = new();
    XElement? itemElement = element.Element("Resource");
    if (itemElement != null)
    {
      Item newItem = Item.FromElement(itemElement) ?? throw new InvalidOperationException("Item element is missing or invalid.");
      trait.Item = newItem;
    }
    return trait;
  }
}
