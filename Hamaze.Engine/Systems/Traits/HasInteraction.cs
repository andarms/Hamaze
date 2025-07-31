using Hamaze.Engine.Core;
using Hamaze.Engine.Events;

namespace Hamaze.Engine.Systems.Traits;

public class HasInteraction() : Trait("HasInteraction")
{
  public Signal OnInteraction { get; } = new();
  public Directions? Side { get; set; } = null;
}