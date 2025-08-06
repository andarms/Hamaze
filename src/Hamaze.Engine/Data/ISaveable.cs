using System.Xml.Linq;

namespace Hamaze.Engine.Data;

public interface ISaveable
{
  XElement Serialize();
  void Deserialize(XElement data);
}
