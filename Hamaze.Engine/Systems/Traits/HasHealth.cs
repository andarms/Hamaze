using Hamaze.Engine.Components.Attack;

namespace Hamaze.Engine.Systems.Traits;

public class HasHealth(Health health) : Trait
{
  public Health Health { get; } = health;
}