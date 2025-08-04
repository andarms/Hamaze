

using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;
namespace Hamaze.Engine.Components.Attack;

public class Hurtbox : GameObject
{
  private readonly Health health;

  public Hurtbox(Health health)
  {
    this.health = health;

    TriggerZone zone = new();
    zone.OnEnter.Connect(HandleCollision);

    this.AddTrait(new HasTriggerZone(zone));

    CollisionsManager.AddObject(this);
  }

  private void HandleCollision(GameObject other)
  {
    if (other is Hitbox hitbox)
    {
      health.TakeDamage(hitbox.DamageCalculator.CalculateDamage());
    }
  }
}


