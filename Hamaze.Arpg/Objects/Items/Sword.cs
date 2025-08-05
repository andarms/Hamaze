using System;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Inventory;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Items;

public class SwordItem : Item
{
  public SwordItem()
  {
    Name = "Sword";
    Description = "A sharp blade for cutting down enemies.";
    Sprite = new Sprite()
    {
      Texture = AssetsManager.Textures["Sprites/TinyDungeon"],
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(144, 144, 16, 16)
    };
    InventorySprite = new Sprite()
    {
      Texture = AssetsManager.Textures["Sprites/TinyDungeon"],
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(144, 144, 16, 16)
    };
  }

  public override void Use()
  {
    throw new NotImplementedException();
  }
}