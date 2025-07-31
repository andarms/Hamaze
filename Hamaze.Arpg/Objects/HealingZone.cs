using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class HealingZone : GameObject
{
  const int HEALING_AMOUNT = 10;
  public HealingZone()
  {
    Name = "Healing Zone";
    Collider = new Collider(
      offset: new Vector2(0, 0),
      size: new Vector2(64)
    );


    TriggerZone zone = new();
    zone.OnEnter.Connect(HandleEnter);
    this.AddTrait(new HasTriggerZone(zone));

    CollisionsManager.AddObject(this);
  }

  private void HandleEnter(GameObject other)
  {
    Health health = other.GetTrait<HasHealth>()?.Health;
    if (health != null && !health.IsDead)
    {
      health.Heal(HEALING_AMOUNT);
    }
  }
}
