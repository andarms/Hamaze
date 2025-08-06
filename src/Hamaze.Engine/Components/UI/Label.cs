using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Components.UI;

public class Label(string text) : GameObject
{
  public string Text { get; set; } = text;
  public Color TextColor { get; set; } = Color.White;
  public Color OutlineColor { get; set; } = Color.Black;
  public int OutlineThickness { get; set; } = 0;
  public float Rotation { get; set; } = 0f;
  public Vector2 Origin { get; set; } = Vector2.Zero;
  public float Scale { get; set; } = 1f;

  public override void Draw(Renderer renderer)
  {
    if (OutlineThickness > 0)
    {
      renderer.DrawTextWithOutline(Text, GlobalPosition, TextColor, OutlineColor, OutlineThickness, Rotation, Origin, Scale);
    }
    else
    {
      renderer.DrawText(Text, GlobalPosition, TextColor, Rotation, Origin, Scale);
    }
  }
}
