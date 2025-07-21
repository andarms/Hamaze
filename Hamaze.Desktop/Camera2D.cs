using System;
using Microsoft.Xna.Framework;

namespace SmoothCameraPixelArt
{
  public class Camera2D
  {
    public Vector2 Position;

    public Matrix GetViewMatrix()
    {
      var roundedPos = new Vector2(
          (float)Math.Floor(Position.X),
          (float)Math.Floor(Position.Y)
      );

      return Matrix.CreateTranslation(new Vector3(-roundedPos, 0f));
    }

  }
}
