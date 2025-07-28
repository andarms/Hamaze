using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Collisions;

public class DynamicObject : GameObject
{

  public override void Initialize()
  {
    base.Initialize();
    CollisionsManager.AddObject(this);
  }

  public void Move(Vector2 movement)
  {
    Position = new Vector2(Position.X + movement.X, Position.Y);
    foreach (var collision in CollisionsManager.GetPotentialCollisions(this))
    {
      CollisionsManager.ResolveSolidCollision(this, collision, true, false);
    }
    Position = new Vector2(Position.X, Position.Y + movement.Y);
    foreach (var collision in CollisionsManager.GetPotentialCollisions(this))
    {
      CollisionsManager.ResolveSolidCollision(this, collision, false, true);
    }
  }

  public override void Draw(Renderer renderer)
  {
    base.Draw(renderer);
    if (Collider == null) return;
    renderer.Shapes.DrawRectangle(
      xy: Position,
      size: Collider.Size,
      c1: new Color(0, 0, 0, 100),
      c2: Color.Red,
      thickness: 2
    );
  }
}