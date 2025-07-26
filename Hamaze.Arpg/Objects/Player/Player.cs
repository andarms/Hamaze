
using Hamaze.Arpg.Content;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Components.UI;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class Player : DynamicObject
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
    };
    AddChild(movement);

    WobbleMovementAnimation wobbleAnimation = new(sprite, this);
    AddChild(wobbleAnimation);

    Health health = new(100, 100);
    Hurtbox hurtbox = new(health);
    AddChild(hurtbox);
    Label healthLabel = new($"Health: {health.Current}/{health.Max}")
    {
      Position = new Vector2(0, -24),
      TextColor = Color.White,
      OutlineColor = Color.Black,
      OutlineThickness = 2,
    };
    health.HealthChanged.Connect((currentHealth) =>
    {
      healthLabel.Text = $"Health: {currentHealth}/{health.Max}";
    });



    AddChild(healthLabel);
  }
}