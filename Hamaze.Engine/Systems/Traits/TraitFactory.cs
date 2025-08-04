using System.Xml.Linq;

namespace Hamaze.Engine.Systems.Traits;

public static class TraitFactory
{
  public static Trait? CreateFromType(string element)
  {
    return element switch
    {
      "IsSolid" => new IsSolid(),
      _ => null // Unknown trait type
    };
  }
}
