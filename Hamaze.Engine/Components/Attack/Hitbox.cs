using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;


namespace Hamaze.Engine.Components.Attack;

public class Hitbox : GameObject
{
  public IDamageCalculator Damage { get; set; } = new NoDamage();

  public Hitbox()
  {
    CollisionsManager.AddObject(this);
  }
}