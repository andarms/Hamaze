using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Systems.Inventory;

public interface IItem
{
  string Name { get; }
  string Description { get; }
  // <summary>
  // The sprite used in the game world.
  // </summary>
  Sprite? Sprite { get; }
  Sprite? InventorySprite { get; }

  abstract void Use();
}