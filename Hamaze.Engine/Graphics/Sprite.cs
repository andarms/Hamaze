using System;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Sprite : GameObject
{
  [Save]
  public Texture2D Texture { get; set; } = null!;

  [Save]
  public Color Color { get; set; } = Color.White;

  [Save]
  public Rectangle? Source { get; set; } = null;

  [Save]
  public Vector2 Origin { get; set; } = Vector2.Zero;

  [Save]
  public float Rotation { get; set; } = 0f;

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
