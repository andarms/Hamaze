using System.Xml.Linq;

namespace Hamaze.Engine.Data;

abstract class Resource
{
  public void Deserialize(XElement data)
  {
  }

  public XElement Serialize()
  {
    return new XElement(GetType().Name);
  }
}