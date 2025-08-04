using System;
using System.Globalization;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Data;

public static class XmlValidationHelper
{
  public static bool TryParseFloat(string? value, float defaultValue, out float result)
  {
    if (string.IsNullOrEmpty(value))
    {
      result = defaultValue;
      return true;
    }

    return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
  }

  public static bool TryParseInt(string? value, int defaultValue, out int result)
  {
    if (string.IsNullOrEmpty(value))
    {
      result = defaultValue;
      return true;
    }

    return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
  }

  public static bool TryParseByte(string? value, byte defaultValue, out byte result)
  {
    if (string.IsNullOrEmpty(value))
    {
      result = defaultValue;
      return true;
    }

    return byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
  }

  public static Vector2 SafeParseVector2(XElement? element, Vector2 defaultValue = default)
  {
    if (element == null) return defaultValue;

    if (TryParseFloat(element.Attribute("X")?.Value, defaultValue.X, out float x) &&
        TryParseFloat(element.Attribute("Y")?.Value, defaultValue.Y, out float y))
    {
      return new Vector2(x, y);
    }

    return defaultValue;
  }

  public static Color SafeParseColor(XElement? element, Color? defaultValue = null)
  {
    var def = defaultValue ?? Color.White;
    if (element == null) return def;

    if (TryParseByte(element.Attribute("R")?.Value, def.R, out byte r) &&
        TryParseByte(element.Attribute("G")?.Value, def.G, out byte g) &&
        TryParseByte(element.Attribute("B")?.Value, def.B, out byte b) &&
        TryParseByte(element.Attribute("A")?.Value, def.A, out byte a))
    {
      return new Color(r, g, b, a);
    }

    return def;
  }

  public static Rectangle SafeParseRectangle(XElement? element, Rectangle defaultValue = default)
  {
    if (element == null) return defaultValue;

    if (TryParseInt(element.Attribute("X")?.Value, defaultValue.X, out int x) &&
        TryParseInt(element.Attribute("Y")?.Value, defaultValue.Y, out int y) &&
        TryParseInt(element.Attribute("Width")?.Value, defaultValue.Width, out int width) &&
        TryParseInt(element.Attribute("Height")?.Value, defaultValue.Height, out int height))
    {
      return new Rectangle(x, y, width, height);
    }

    return defaultValue;
  }


  public static Texture2D SafeParseTexture(XElement? element)
  {
    ArgumentNullException.ThrowIfNull(element);
    var texturePath = element.Element("TexturePath")?.Value;
    if (string.IsNullOrEmpty(texturePath))
    {
      throw new InvalidOperationException("TexturePath element is missing or empty.");
    }
    return AssetsManager.Textures[texturePath] ?? throw new InvalidOperationException($"Texture not found: {texturePath}");
  }

  public static XElement SerializeTexture(Texture2D texture, string elementName)
  {
    ArgumentNullException.ThrowIfNull(texture);
    XElement property = new(elementName);
    property.Add(new XElement("TexturePath", texture.Name));
    return property;
  }

  public static float SafeParseFloat(string? value, float defaultValue = 0f)
  {
    if (TryParseFloat(value, defaultValue, out float result))
    {
      return result;
    }
    return defaultValue;
  }
}
