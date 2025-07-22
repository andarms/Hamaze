using Hamaze.Arpg.Content;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class Player : GameObject
{

  public Player()
  {
    Sprite sprite = new(AssetsManager.TinyDungeon)
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = new Rectangle(16, 112, 16, 16)
    };
    AddChild(sprite);

    Movement movement = new(player: this)
    {
      Speed = 4f
    };
    AddChild(movement);
  }
}