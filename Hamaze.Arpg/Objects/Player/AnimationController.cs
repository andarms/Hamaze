using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;

namespace Hamaze.Arpg.Objects.Player;

public class AnimationController(SpriteSheet sheet) : GameObject
{
  public enum Direction
  {
    Down,
    Up,
    Left,
    Right
  }

  public enum AnimationType
  {
    Idle,
    Walk
  }

  public Animation WalkDown { get; private set; } = new(sheet, [4, 8, 12, 8]) { Speed = 180f, };
  public Animation WalkUp { get; private set; } = new(sheet, [5, 9, 13, 9]) { Speed = 180f, };
  public Animation WalkLeft { get; private set; } = new(sheet, [6, 10, 14, 10]) { Speed = 180f, };
  public Animation WalkRight { get; private set; } = new(sheet, [7, 11, 15, 11]) { Speed = 180f, };

  public Animation IdleDown { get; private set; } = new(sheet, [0]) { Speed = 180f, };
  public Animation IdleUp { get; private set; } = new(sheet, [1]) { Speed = 180f, };
  public Animation IdleLeft { get; private set; } = new(sheet, [2]) { Speed = 180f, };
  public Animation IdleRight { get; private set; } = new(sheet, [3]) { Speed = 180f, };

  private Animation currentAnimation = null!;

  public override void Initialize()
  {
    base.Initialize();
    AddChild(WalkDown);
    AddChild(WalkUp);
    currentAnimation = WalkUp;
  }

  public override void Update(float dt)
  {
    currentAnimation.Update(dt);
  }

  public void SetAnimation(AnimationType type, Direction direction)
  {
    switch (type)
    {
      case AnimationType.Walk:
        currentAnimation = direction switch
        {
          Direction.Down => WalkDown,
          Direction.Up => WalkUp,
          Direction.Left => WalkLeft,
          Direction.Right => WalkRight,
          _ => currentAnimation
        };
        break;
      case AnimationType.Idle:
        currentAnimation = direction switch
        {
          Direction.Down => IdleDown,
          Direction.Up => IdleUp,
          Direction.Left => IdleLeft,
          Direction.Right => IdleRight,
          _ => currentAnimation
        };
        break;
    }
  }

  public override void Draw(Renderer renderer)
  {
    currentAnimation.Draw(renderer);
  }
}