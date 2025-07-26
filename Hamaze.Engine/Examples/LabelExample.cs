using Hamaze.Engine.Components.UI;
using Microsoft.Xna.Framework;

namespace Hamaze.Examples;

/// <summary>
/// Example demonstrating how to use the enhanced Label component with outline
/// </summary>
public static class LabelExample
{
  public static Label CreateSimpleLabel()
  {
    return new Label("Hello World!")
    {
      Position = new Vector2(100, 50),
      TextColor = Color.White
    };
  }

  public static Label CreateLabelWithOutline()
  {
    return new Label("Outlined Text!")
    {
      Position = new Vector2(100, 100),
      TextColor = Color.Yellow,
      OutlineColor = Color.Black,
      OutlineThickness = 2
    };
  }

  public static Label CreateStyledLabel()
  {
    return new Label("Styled Label")
    {
      Position = new Vector2(100, 150),
      TextColor = Color.Cyan,
      OutlineColor = Color.DarkBlue,
      OutlineThickness = 1,
      Scale = 1.5f,
      Rotation = 0.1f // Slight rotation
    };
  }

  public static Label CreateThickOutlineLabel()
  {
    return new Label("THICK OUTLINE")
    {
      Position = new Vector2(100, 200),
      TextColor = Color.White,
      OutlineColor = Color.Red,
      OutlineThickness = 3,
      Scale = 2f
    };
  }
}
