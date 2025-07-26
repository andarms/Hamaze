using System;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class SpriteSheet(Texture2D texture, int frameWidth, int frameHeight) : GameObject
{
  public Texture2D Texture { get; } = texture;
  public int FrameWidth { get; } = frameWidth;
  public int FrameHeight { get; } = frameHeight;
  public int TotalFrames { get; } = texture.Width / frameWidth * (texture.Height / frameHeight);

  public Sprite sprite = new(texture);


  public override void Initialize()
  {
    base.Initialize();
    sprite.Origin = Vector2.Zero;
    sprite.Color = Color.White;
    sprite.Source = GetFrame(0);
    AddChild(sprite);
  }

  public Rectangle GetFrame(int index)
  {
    int columns = Texture.Width / FrameWidth;
    int x = index % columns * FrameWidth;
    int y = index / columns * FrameHeight;
    return new Rectangle(x, y, FrameWidth, FrameHeight);
  }

  public void SetFrame(int index)
  {
    if (index < 0 || index >= TotalFrames)
    {
      throw new ArgumentOutOfRangeException(nameof(index), "Frame index is out of range.");
    }
    sprite.Source = GetFrame(index);
  }

  public override void Draw(Renderer renderer)
  {
    renderer.DrawSprite(sprite);
  }
}
