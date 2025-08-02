using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Spikes : GameObject
{
  public Spikes()
  {
    Name = "Spikes";
    Sprite sprite = new(AssetsManager.Textures["Sprites/TinyDungeon"])
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(80, 48, 16, 16)
    };
    AddChild(sprite);

    Vector2 size = new Vector2(16) * Renderer.ScaleFactor;

    Hitbox hitbox = new()
    {
      Damage = new SimpleDamage(10),
      Collider = new(
        offset: new Vector2(0, 0),
        size: size
      )
    };
    AddChild(hitbox);
  }
}
