using Hamaze.Arpg.Content;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class Player : KinematicBody
{
  public Player()
  {
    Name = "Player";
    Sprite sprite = new(AssetsManager.TinyDungeon)
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(16, 112, 16, 16)
    };
    AddChild(sprite);

    Vector2 size = new Vector2(16, 16) * Renderer.ScaleFactor;
    Collider collider = new(new(0, 0, (int)size.X, (int)size.Y));
    AddChild(collider);

    Movement movement = new(player: this)
    {
      Speed = 4f
    };
    AddChild(movement);

    WobbleMovementAnimation wobbleAnimation = new(sprite, movement);
    AddChild(wobbleAnimation);
  }
}