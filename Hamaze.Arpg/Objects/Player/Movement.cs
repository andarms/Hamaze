using System;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg.Objects.Player;

public class Movement(Player player) : GameObject
{
  public float Speed { get; set; } = 2;
  public Vector2 Velocity { get; set; } = Vector2.Zero;

  public override void Update(float dt)
  {
    base.Update(dt);
    Velocity = Vector2.Zero;

    if (Keyboard.GetState().IsKeyDown(Keys.Up))
      Velocity -= new Vector2(0, 1);
    if (Keyboard.GetState().IsKeyDown(Keys.Down))
      Velocity += new Vector2(0, 1);
    if (Keyboard.GetState().IsKeyDown(Keys.Left))
      Velocity -= new Vector2(1, 0);
    if (Keyboard.GetState().IsKeyDown(Keys.Right))
      Velocity += new Vector2(1, 0);

    if (Velocity != Vector2.Zero)
    {
      Velocity.Normalize();
    }
    player.Position += Velocity * Speed;

  }
}