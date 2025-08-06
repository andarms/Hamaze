using System;
using System.Xml.Linq;

namespace Hamaze.Engine.Systems.Traits;

// <summary>
// Traits are lightweight data containers that define the state or identity of a game object.
// They do not contain logic, they're purely declarative and can be added or removed dynamically at runtime.
// </summary>
public class Trait
{
  public string Type => GetType().Name;

  public virtual XElement Serialize()
  {
    XElement element = new("Trait");
    element.SetAttributeValue("Type", Type);
    return element;
  }

  public virtual void Deserialize(XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
  }
}
