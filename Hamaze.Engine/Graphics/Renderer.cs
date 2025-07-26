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
  private readonly SpriteFont DefaultFont = content.Load<SpriteFont>("Fonts/Default");


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

  public void DrawText(string text, Vector2 position, Color color)
  {
    Batch.DrawString(DefaultFont, text, position * ScaleFactor, color);
  }

  public void DrawText(string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
  {
    Batch.DrawString(DefaultFont, text, position, color, rotation, origin, scale, SpriteEffects.None, 0f);
  }

  public void DrawTextWithOutline(string text, Vector2 position, Color textColor, Color outlineColor, int outlineThickness, float rotation = 0f, Vector2 origin = default, float scale = 1f)
  {
    // Draw outline
    for (int x = -outlineThickness; x <= outlineThickness; x++)
    {
      for (int y = -outlineThickness; y <= outlineThickness; y++)
      {
        if (x == 0 && y == 0) continue; // Skip center position

        Vector2 outlineOffset = new Vector2(x, y);
        Batch.DrawString(DefaultFont, text, position + outlineOffset, outlineColor, rotation, origin, scale, SpriteEffects.None, 0f);
      }
    }

    // Draw main text on top
    Batch.DrawString(DefaultFont, text, position, textColor, rotation, origin, scale, SpriteEffects.None, 0f);
  }

  public void End()
  {
    Shapes.End();
    Batch.End();
  }
}