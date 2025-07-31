using Hamaze.Engine.Core;

namespace Hamaze.Engine.Systems.Traits;

public class HasTriggerZone(TriggerZone zone) : Trait("HasTriggerZone")
{
  public TriggerZone Zone { get; } = zone;
}
