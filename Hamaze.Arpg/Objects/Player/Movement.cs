using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg.Objects.Player;

public class Movement(Player player) : GameObject
{
  public float Speed { get; set; } = 100;

  public override void Update(float dt)
  {
    base.Update(dt);
    var direction = InputManager.GetVector("move_left", "move_right", "move_up", "move_down");

    var stickInput = InputManager.GetGamepadLeftStick();
    if (stickInput.Length() > 0.1f)
    {
      direction = stickInput;
    }

    if (direction != Vector2.Zero)
    {
      direction.Normalize();
    }

    float currentSpeed = Speed;
    if (InputManager.IsActionPressed("run"))
    {
      currentSpeed *= 2f;
    }
    player.Velocity = direction * currentSpeed;
  }
}