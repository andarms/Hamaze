

using Hamaze.Arpg.Content;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Components.UI;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class PlayerTrait() : Trait("Player") { }

public class Player : DynamicObject
{
  public Health Health = new(100, 100);
  public AnimationController Animations { get; private set; }
  public Vector2 FacingDirection { get; set; } = Vector2.Zero;

  public Player()
  {
    Name = "Player";
    AddComponents();
    AddTraits();
  }

  private void AddComponents()
  {
    SpriteSheet sheet = new(AssetsManager.Boy, 16, 16);
    AddChild(sheet);

    Animations = new(sheet);
    AddChild(Animations);

    Collider = new Collider(
      offset: new Vector2(16, 32),
      size: new Vector2(32, 32)
    );

    Movement movement = new(player: this) { Speed = 150f };
    AddChild(movement);

    Hurtbox hurtbox = new(Health)
    {
      Collider = Collider
    };
    AddChild(hurtbox);

    Label healthLabel = new(Health.ToString())
    {
      Position = new Vector2(0, -24),
      TextColor = Color.White,
      OutlineColor = Color.Black,
      OutlineThickness = 2,
    };
    Health.HealthChanged.Connect((currentHealth) =>
    {
      healthLabel.Text = Health.ToString();
    });

    AddChild(healthLabel);

    Interaction interaction = new(this);
    AddChild(interaction);
  }

  private void AddTraits()
  {
    Traits.Add(new PlayerTrait());
    Traits.Add(new Solid());
    Traits.Add(Health);
  }
}