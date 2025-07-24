using System;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Renderer(GraphicsDevice graphicsDevice, ContentManager content)
{
  public readonly ShapeBatch Shapes = new(graphicsDevice, content);
  private readonly SpriteBatch Batch = new(graphicsDevice);


  public const int ScaleFactor = 4;

  public void Begin()
  {
    Batch.Begin(samplerState: SamplerState.PointClamp);
    Shapes.Begin();
  }

  public void ClearBackground(Color color)
  {
    Batch.GraphicsDevice.Clear(color);
  }

  public void DrawSprite(Sprite sprite)
  {
    if (sprite.Texture == null)
    {
      throw new ArgumentNullException(nameof(sprite.Texture), "Sprite texture cannot be null.");
    }

    Batch.Draw(
      sprite.Texture,
      sprite.GlobalPosition + sprite.Origin * ScaleFactor,
      sprite.Source,
      sprite.Color,
      sprite.Rotation,
      sprite.Origin,
      ScaleFactor,
      SpriteEffects.None,
      0f
    );
  }

  public void End()
  {
    Shapes.End();
    Batch.End();
  }
}