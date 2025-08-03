using System;
using System.Collections.Generic;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class NinePatchSprite(Texture2D texture, int left, int right, int top, int bottom) : GameObject
{
  public Texture2D TexturePath { get; } = texture;
  public int Left { get; } = left;
  public int Right { get; } = right;
  public int Top { get; } = top;
  public int Bottom { get; } = bottom;

  public Vector2 Size { get; set; } = new(texture.Width, texture.Height);


  readonly List<Rectangle> sources =
  [
    new (0, 0, left, top), // Top-left corner
    new (left, 0, right - left, top), // Top edge
    new (right, 0, texture.Width - right, top), // Top-right corner
    new (0, top, left, bottom - top), // Left edge
    new (left, top, right - left, bottom - top), // Center
    new (right, top, texture.Width - right, bottom - top), // Right edge
    new (0, bottom, left, texture.Height - bottom), // Bottom-left corner
    new (left, bottom, right - left, texture.Height - bottom), // Bottom edge
    new (right, bottom, texture.Width - right, texture.Height - bottom) // Bottom-right corner
  ];

  public NinePatchSprite(Texture2D texturePath, int padding) : this(texturePath, padding, padding, padding, padding)
  {
  }


  public override void Draw(Renderer renderer)
  {
    base.Draw(renderer);
    // Calculate destination rectangles for each patch based on Size
    float width = Size.X;
    float height = Size.Y;

    int left = Left;
    int right = Right;
    int top = Top;
    int bottom = Bottom;

    float centerWidth = width - (left + (TexturePath.Width - right));
    float centerHeight = height - (top + (TexturePath.Height - bottom));

    // Ensure we don't have negative sizes
    centerWidth = Math.Max(0, centerWidth);
    centerHeight = Math.Max(0, centerHeight);

    // Apply scale factor to position and dimensions
    int scaledX = (int)(Position.X);
    int scaledY = (int)(Position.Y);
    int scaledLeft = left * Renderer.ScaleFactor;
    int scaledRight = (TexturePath.Width - right) * Renderer.ScaleFactor;
    int scaledTop = top * Renderer.ScaleFactor;
    int scaledBottom = (TexturePath.Height - bottom) * Renderer.ScaleFactor;
    int scaledCenterWidth = (int)centerWidth - ((left + right) * Renderer.ScaleFactor);
    int scaledCenterHeight = (int)centerHeight - ((top + bottom) * Renderer.ScaleFactor);

    // Top row
    Rectangle[] destRects =
    [
      new Rectangle(scaledX, scaledY, scaledLeft, scaledTop), // Top-left
      new Rectangle(scaledX + scaledLeft, scaledY, scaledCenterWidth, scaledTop), // Top
      new Rectangle(scaledX + scaledLeft + scaledCenterWidth, scaledY, scaledRight, scaledTop), // Top-right

      // Middle row
      new Rectangle(scaledX, scaledY + scaledTop, scaledLeft, scaledCenterHeight), // Left
      new Rectangle(scaledX + scaledLeft, scaledY + scaledTop, scaledCenterWidth, scaledCenterHeight), // Center
      new Rectangle(scaledX + scaledLeft + scaledCenterWidth, scaledY + scaledTop, scaledRight, scaledCenterHeight), // Right

      // Bottom row
      new Rectangle(scaledX, scaledY + scaledTop + scaledCenterHeight, scaledLeft, scaledBottom), // Bottom-left
      new Rectangle(scaledX + scaledLeft, scaledY + scaledTop + scaledCenterHeight, scaledCenterWidth, scaledBottom), // Bottom
      new Rectangle(scaledX + scaledLeft + scaledCenterWidth, scaledY + scaledTop + scaledCenterHeight, scaledRight, scaledBottom) // Bottom-right
    ];

    for (int i = 0; i < 9; i++)
    {
      // Only draw patches that have positive width and height
      if (destRects[i].Width > 0 && destRects[i].Height > 0)
      {
        renderer.DrawSprite(TexturePath, destRects[i], sources[i], Color.White);
      }
    }
  }


}