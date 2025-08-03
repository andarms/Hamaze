using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Data;

[XmlRoot("GameObject")]
public class GameObjectData
{
  [XmlElement("Position")]
  public Vector2Data Position { get; set; } = null!;

  [XmlElement("Name")]
  public string Name { get; set; } = string.Empty;

  [XmlElement("Collider")]
  public ColliderData? Collider { get; set; }
}

public class Vector2Data
{
  [XmlAttribute("x")]
  public float X { get; set; }

  [XmlAttribute("y")]
  public float Y { get; set; }

  public Vector2 ToVector2() => new(X, Y);
}


public class ColliderData
{
  [XmlElement("Offset")]
  public Vector2Data Offset { get; set; } = null!;

  [XmlElement("Size")]
  public Vector2Data Size { get; set; } = null!;
}