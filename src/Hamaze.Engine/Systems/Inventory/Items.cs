using System;
using System.Xml.Linq;
using Hamaze.Engine.Data;
using Hamaze.Engine.Graphics;

namespace Hamaze.Engine.Systems.Inventory;

public class Item : Resource, IUsable
{
  [Save]
  public string Name { get; protected set; } = String.Empty;
  [Save]
  public string Description { get; protected set; } = String.Empty;
  [Save]
  public Sprite? Sprite { get; protected set; } = null;
  [Save]
  public Sprite? InventorySprite { get; protected set; } = null;

  public virtual void Use()
  {
    // Default implementation does nothing
    // Override in derived classes for specific behavior
  }

  // public override void Deserialize(XElement data)
  // {
  //   Name = data.Element("Name")?.Value ?? String.Empty;
  //   Description = data.Element("Description")?.Value ?? String.Empty;
  //   Sprite = new Sprite();
  //   InventorySprite = new Sprite();
  //   XElement? parentSpriteElement = data.Element("Sprite");
  //   if (parentSpriteElement != null)
  //   {
  //     XElement spriteElement = parentSpriteElement.Element("Sprite") ?? throw new InvalidOperationException("Sprite element is missing or invalid.");
  //     Sprite.Deserialize(spriteElement);
  //   }
  //   XElement? parentInventorySpriteElement = data.Element("InventorySprite");
  //   if (parentInventorySpriteElement != null)
  //   {
  //     XElement inventorySpriteElement = parentInventorySpriteElement.Element("Sprite") ?? throw new InvalidOperationException("InventorySprite element is missing or invalid.");
  //     InventorySprite.Deserialize(inventorySpriteElement);
  //   }
  // }

  public static Item FromElement(XElement? element)
  {
    if (element == null)
    {
      ArgumentNullException.ThrowIfNull(element);
    }

    Item item = new();
    item.Deserialize(element);
    return item;
  }
}