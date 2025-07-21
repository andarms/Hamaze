using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Sprite(Texture2D texture) : GameObject
{
  public Texture2D Texture { get; } = texture;
  public Color Color { get; set; } = Color.White;
  public Rectangle? Source { get; set; } = null;
  public Vector2 Origin { get; set; } = Vector2.Zero;
  public float Rotation { get; set; } = 0f;



  public override void Draw(Renderer renderer)
  {
    base.Draw(renderer);
    renderer.DrawSprite(this);
  }
}