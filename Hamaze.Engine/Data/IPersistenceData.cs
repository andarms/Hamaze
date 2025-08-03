using System.Xml.Linq;

namespace Hamaze.Engine.Data;

public interface IPersistenceData
{
  XElement Serialize();
  void Deserialize(XElement data);
}
