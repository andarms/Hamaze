using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Components.UI;

public class Label(string text) : GameObject
{
  public string Text { get; set; } = text;


  public override void Draw(Renderer renderer)
  {
    renderer.DrawText(Text, GlobalPosition, Color.White, 0f, Vector2.Zero, 1f);
  }
}
