using Hamaze.Engine.Events;

namespace Hamaze.Engine.Core;

public class Interactable() : Trait("Interactable")
{
  public Signal OnInteraction { get; } = new();
}