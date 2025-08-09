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
  readonly TriggerZone zone;

  public HealingZone()
  {
    Name = "Healing Zone";
    Collider = new Collider(
      offset: new Vector2(0, 0),
      size: new Vector2(64)
    );

    zone = new TriggerZone();
    zone.OnEnter.Connect(HandleEnter);
    this.AddTrait(new HasTriggerZone(zone));

    CollisionsManager.AddObject(this);
  }


  public override void Initialize()
  {
    base.Initialize();

  }

  private void HandleEnter(GameObject other)
  {
    Health health = other.Trait<HasHealth>()?.Health;
    if (health != null && !health.IsDead)
    {
      health.Heal(HEALING_AMOUNT);
    }
  }
}
