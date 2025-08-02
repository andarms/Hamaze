using Hamaze.Engine.Graphics;

namespace Hamaze.Engine.Systems.Inventory;

public interface IItem
{
  string Name { get; }
  string Description { get; }
  Sprite? Sprite { get; }
  Sprite? InventorySprite { get; }
  abstract void Use();
}