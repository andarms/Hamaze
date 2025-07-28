

using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Components.Attack;

public class Hurtbox : GameObject
{
  private readonly Health health;

  public Hurtbox(Health health)
  {
    this.health = health;

    TriggerZone zone = new();
    zone.OnEnter.Connect(HandleCollision);
    Traits.Add(zone);

    CollisionsManager.AddObject(this);
  }

  private void HandleCollision(GameObject other)
  {
    if (other is Hitbox hitbox)
    {
      health.TakeDamage(hitbox.Damage.CalculateDamage());
    }
  }
}


