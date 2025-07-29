using Hamaze.Engine.Collisions;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class HealingZone : GameObject
{
  public HealingZone()
  {
    Name = "Healing Zone";
    Collider = new Collider(
      offset: new Vector2(0, 0),
      size: new Vector2(64)
    );


    TriggerZone zone = new();
    zone.OnEnter.Connect(HandleEnter);
    Traits.Add(zone);
    CollisionsManager.AddObject(this);
  }

  private void HandleEnter(GameObject other)
  {
    if (other.Traits.Has<Health>())
    {
      other.Traits.Get<Health>().Heal(10);
    }
  }
}
