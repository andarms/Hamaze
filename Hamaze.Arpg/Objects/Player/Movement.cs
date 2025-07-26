using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hamaze.Arpg.Objects.Player;

public class Movement(Player player) : GameObject
{
  public float Speed { get; set; } = 100;

  AnimationController.Direction animationDirection = AnimationController.Direction.Down;
  AnimationController.Direction lastDirection = AnimationController.Direction.Down;
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
      if (Math.Abs(direction.X) > Math.Abs(direction.Y))
      {
        animationDirection = direction.X > 0 ? AnimationController.Direction.Right : AnimationController.Direction.Left;
      }
      else
      {
        animationDirection = direction.Y > 0 ? AnimationController.Direction.Down : AnimationController.Direction.Up;
      }
      lastDirection = animationDirection;
      animationType = AnimationController.AnimationType.Walk;
      direction.Normalize();
    }
    else
    {
      // Use last direction when idle
      animationDirection = lastDirection;
      animationType = AnimationController.AnimationType.Idle;
    }

    float currentSpeed = Speed;
    if (InputManager.IsActionPressed("run"))
    {
      currentSpeed *= 2f;
    }

    player.Animations.SetAnimation(animationType, animationDirection);
    player.Velocity = direction * currentSpeed;
  }
}