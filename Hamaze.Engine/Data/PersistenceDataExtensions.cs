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
    return XmlValidationHelper.SafeParseVector2(element);
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

  public static Color DeserializeColor(XElement element)
  {
    return XmlValidationHelper.SafeParseColor(element);
  }

  public static void Deserialize(this Color data, XElement element)
  {
    var color = DeserializeColor(element);
    data.R = color.R;
    data.G = color.G;
    data.B = color.B;
    data.A = color.A;
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

  public static Rectangle DeserializeRectangle(XElement element)
  {
    return XmlValidationHelper.SafeParseRectangle(element);
  }

  public static void Deserialize(this Rectangle data, XElement element)
  {
    var rect = DeserializeRectangle(element);
    data.X = rect.X;
    data.Y = rect.Y;
    data.Width = rect.Width;
    data.Height = rect.Height;
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