using System;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Sprite : GameObject
{
  public Texture2D Texture { get; set; } = null!;
  public Color Color { get; set; } = Color.White;
  public Rectangle? Source { get; set; } = null;
  public Vector2 Origin { get; set; } = Vector2.Zero;
  public float Rotation { get; set; } = 0f;

  public Sprite()
  {
    SerializableState = new SpriteSerializer(this);
  }

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Texture);
  }

  public override void Draw(Renderer renderer)
  {
    renderer.DrawSprite(this);
    base.Draw(renderer);
  }
}
