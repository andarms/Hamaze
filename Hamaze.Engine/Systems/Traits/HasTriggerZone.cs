using Hamaze.Engine.Core;

namespace Hamaze.Engine.Systems.Traits;

public class HasTriggerZone(TriggerZone zone) : Trait
{
  public TriggerZone Zone { get; } = zone;
}
