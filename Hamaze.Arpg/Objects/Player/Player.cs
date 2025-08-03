using System.Xml.Serialization;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Components.UI;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Inventory;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class IsPlayer() : Trait("Player") { }


public class Player : DynamicObject
{
  public Health Health = new(100, 100);
  public AnimationController Animations { get; private set; }
  public Directions FacingDirection { get; set; } = Directions.Down;

  public Inventory Inventory { get; } = new();

  public Player()
  {
    Name = "Player";
    AddComponents();
    AddTraits();
  }

  private void AddComponents()
  {
    SpriteSheet sheet = new(AssetsManager.Textures["Sprites/NinjaGreen/SpriteSheet"], 16, 16);
    AddChild(sheet);

    Animations = new(sheet);
    AddChild(Animations);

    Collider = new Collider(
      offset: new Vector2(0, 32),
      size: new Vector2(64, 32)
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
    this.AddTrait(new IsPlayer());
    this.AddTrait(new IsSolid());
    this.AddTrait(new HasHealth(Health));
    this.AddTrait(new HasInventory(Inventory));
  }
}