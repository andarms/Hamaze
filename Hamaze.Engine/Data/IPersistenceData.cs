using System.Xml.Linq;

namespace Hamaze.Engine.Data;

public interface ISerializableData
{
  XElement Serialize();
  void Deserialize(XElement data);
}
