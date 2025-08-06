using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg.Objects.Player;

public class Movement(Player player) : GameObject
{
  public float Speed { get; set; } = 100;

  Directions animationDirection = Directions.Down;
  Directions lastDirection = Directions.Down;
  AnimationController.AnimationType animationType = AnimationController.AnimationType.Idle;

  public override void Update(float dt)
  {
    base.Update(dt);
    var direction = InputManager.GetVector("move_left", "move_right", "move_up", "move_down");

    var stickInput = InputManager.GetGamepadLeftStick();
    if (stickInput.Length() > 0.1f)
    {
      direction = stickInput;
    }

    // Update animation direction only when there's input
    if (direction != Vector2.Zero)
    {
      if (direction.Y != 0)
      {
        animationDirection = direction.Y > 0 ? Directions.Down : Directions.Up;
      }
      if (direction.X != 0)
      {

        animationDirection = direction.X > 0 ? Directions.Right : Directions.Left;
      }
      lastDirection = animationDirection;
      animationType = AnimationController.AnimationType.Walk;
      direction.Normalize();
    }
    else
    {
      animationDirection = lastDirection;
      animationType = AnimationController.AnimationType.Idle;
    }

    float currentSpeed = Speed;
    if (InputManager.IsActionPressed("run"))
    {
      currentSpeed *= 2f;
    }

    player.Animations.SetAnimation(animationType, animationDirection);
    var velocity = direction * currentSpeed * dt;
    player.FacingDirection = lastDirection;
    player.Move(velocity);
  }
}