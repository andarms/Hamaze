using System.Collections.Generic;
using System.Linq;

namespace Hamaze.Engine.Core;

public class TraitCollection : HashSet<Trait>
{
  public bool Has<T>() where T : Trait
  {
    return this.Any(t => t is T);
  }

  public bool Has(string name)
  {
    return this.Any(t => t.Name == name);
  }
}


