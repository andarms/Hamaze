using Hamaze.Engine.Physics;

namespace Hamaze.Engine.Components.Attack;

public class Hitbox : TriggerZone
{
  public IDamageCalculator Damage { get; set; } = new NoDamage();
}