using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Data;

public static class PersistenceDataExtensions
{
  public static XElement Serialize(this Vector2 data, string elementName = "Vector2")
  {
    var element = new XElement(elementName);
    element.SetAttributeValue("X", data.X);
    element.SetAttributeValue("Y", data.Y);
    return element;
  }

  public static Vector2 Deserialize(XElement element)
  {
    var x = float.Parse(element.Attribute("X")?.Value ?? "0");
    var y = float.Parse(element.Attribute("Y")?.Value ?? "0");
    return new Vector2(x, y);
  }

  public static XElement Serialize(this Color data, string elementName)
  {
    var element = new XElement(elementName);
    element.SetAttributeValue("R", data.R);
    element.SetAttributeValue("G", data.G);
    element.SetAttributeValue("B", data.B);
    element.SetAttributeValue("A", data.A);
    return element;
  }

  public static void Deserialize(this Color data, XElement element)
  {
    data.R = byte.Parse(element.Attribute("R")?.Value ?? "255");
    data.G = byte.Parse(element.Attribute("G")?.Value ?? "255");
    data.B = byte.Parse(element.Attribute("B")?.Value ?? "255");
    data.A = byte.Parse(element.Attribute("A")?.Value ?? "255");
  }


  public static XElement Serialize(this Rectangle data, string elementName)
  {
    var element = new XElement(elementName);
    element.SetAttributeValue("X", data.X);
    element.SetAttributeValue("Y", data.Y);
    element.SetAttributeValue("Width", data.Width);
    element.SetAttributeValue("Height", data.Height);
    return element;
  }

  public static void Deserialize(this Rectangle data, XElement element)
  {
    data.X = int.Parse(element.Attribute("X")?.Value ?? "0");
    data.Y = int.Parse(element.Attribute("Y")?.Value ?? "0");
    data.Width = int.Parse(element.Attribute("Width")?.Value ?? "0");
    data.Height = int.Parse(element.Attribute("Height")?.Value ?? "0");
  }


  public static XElement Serialize(this Collider collider, string elementName)
  {
    var element = new XElement(elementName);
    element.Add(collider.Offset.Serialize("Offset"));
    element.Add(collider.Size.Serialize("Size"));
    return element;
  }

  public static Collider Deserialize(this Collider collider, XElement element)
  {
    XElement? offsetElement = element.Element("Offset");
    if (offsetElement != null)
    {
      collider.Offset = PersistenceDataExtensions.Deserialize(offsetElement);
    }

    XElement? sizeElement = element.Element("Size");
    if (sizeElement != null)
    {
      collider.Size = PersistenceDataExtensions.Deserialize(sizeElement);
    }
    return collider;
  }
}