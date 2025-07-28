using Hamaze.Engine.Events;

namespace Hamaze.Engine.Core;

public record TriggerZone() : Trait("TriggerZone")
{
  public Signal<GameObject> OnEnter = new();
  public Signal<GameObject> OnExit = new();
}
