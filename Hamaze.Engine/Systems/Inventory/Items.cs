using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Systems.Inventory;

public abstract class Item
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  // <summary>
  // The sprite used in the game world.
  // </summary>
  public Sprite? Sprite { get; set; } = null;
  public Texture2D? InventorySprite { get; set; } = null;

  public abstract void Use();
}