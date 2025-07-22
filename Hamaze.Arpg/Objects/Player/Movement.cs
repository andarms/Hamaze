using System;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg.Objects.Player;

public class Movement(Player player) : GameObject
{
  public float Speed { get; set; } = 1f;

  public override void Update(float dt)
  {
    base.Update(dt);
    var direction = Vector2.Zero;

    if (Keyboard.GetState().IsKeyDown(Keys.Up))
      direction.Y -= 1;
    if (Keyboard.GetState().IsKeyDown(Keys.Down))
      direction.Y += 1;
    if (Keyboard.GetState().IsKeyDown(Keys.Left))
      direction.X -= 1;
    if (Keyboard.GetState().IsKeyDown(Keys.Right))
      direction.X += 1;

    if (direction != Vector2.Zero)
    {
      direction.Normalize();
    }
    player.Position += direction * dt * Speed;

  }
}