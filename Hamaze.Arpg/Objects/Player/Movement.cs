using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
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
    var direction = InputService.GetVector("move_left", "move_right", "move_up", "move_down");

    var stickInput = InputService.GetGamepadLeftStick();
    if (stickInput.Length() > 0.1f)
    {
      direction = stickInput;
    }

    if (direction != Vector2.Zero)
    {
      direction.Normalize();
    }

    float currentSpeed = Speed;
    if (InputService.IsActionPressed("run"))
    {
      currentSpeed *= 2f; // Double speed when running
    }
    Velocity = direction;
    player.Position += direction * currentSpeed;
  }
}