using Hamaze.Engine.Components.Attack;

namespace Hamaze.Engine.Systems.Traits;

public class HasHealth(Health health) : Trait("HasHealth")
{
  public Health Health { get; } = health;
}