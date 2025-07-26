using System;
using Hamaze.Arpg.Content;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Spikes : GameObject
{
  public Spikes()
  {
    Name = "Spikes";
    Sprite sprite = new(AssetsManager.TinyDungeon)
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(80, 48, 16, 16)
    };
    AddChild(sprite);

    Hitbox hitbox = new() { Damage = new SimpleDamage(10) };
    AddChild(hitbox);
    Vector2 size = new Vector2(16) * Renderer.ScaleFactor;
    Collider collider = new(new(0, 0, (int)size.X, (int)size.Y));
    AddChild(collider);

  }
}
