using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Renderer(GraphicsDevice graphicsDevice)
{
  private readonly SpriteBatch Batch = new(graphicsDevice);

  public const int ScaleFactor = 4;

  public void Begin()
  {
    Batch.Begin(samplerState: SamplerState.PointClamp);
  }

  public void DrawSprite(Sprite sprite)
  {
    if (sprite.Texture == null)
    {
      throw new ArgumentNullException(nameof(sprite.Texture), "Sprite texture cannot be null.");
    }

    Batch.Draw(
     sprite.Texture,
     sprite.GlobalPosition,
     sprite.Source,
     sprite.Color,
     sprite.Rotation,
     sprite.Origin,
     ScaleFactor,
     SpriteEffects.None,
     0f);
  }

  public void End()
  {
    Batch.End();
  }
}