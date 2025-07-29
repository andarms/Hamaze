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

  public T? Get<T>() where T : Trait
  {
    return this.FirstOrDefault(t => t is T) as T;
  }

  public Trait? Get(string name)
  {
    return this.FirstOrDefault(t => t.Name == name);
  }

  public void Remove<T>() where T : Trait
  {
    var trait = this.FirstOrDefault(t => t is T);
    if (trait != null)
    {
      Remove(trait);
    }
  }

}


