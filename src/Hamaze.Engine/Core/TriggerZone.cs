using Hamaze.Engine.Events;

namespace Hamaze.Engine.Core;

public class TriggerZone
{
  public Signal<GameObject> OnEnter = new();
  public Signal<GameObject> OnExit = new();
}
