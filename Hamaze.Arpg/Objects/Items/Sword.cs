using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Inventory;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Items;

public class SwordItem : IItem
{
  public string Name => "Sword";

  public string Description { get; } = "A sharp blade for cutting down enemies.";

  public Sprite Sprite { get; } = new Sprite(AssetsManager.Textures["Sprites/TinyDungeon"])
  {
    Origin = new Vector2(8, 16),
    Color = Color.White,
    Source = new Rectangle(144, 144, 16, 16)
  };

  public Sprite InventorySprite { get; } = new Sprite(AssetsManager.Textures["Sprites/TinyDungeon"])
  {
    Origin = new Vector2(8, 16),
    Color = Color.White,
    Source = new Rectangle(144, 144, 16, 16)
  };

  public void Use()
  {
    Console.WriteLine("You swing the sword!");
  }
}