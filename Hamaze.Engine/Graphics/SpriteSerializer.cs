using System;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public static class SpriteSerializer
{
  public static XElement Serialize(Sprite data)
  {
    var element = new XElement("Sprite");
    element.Add(data.Position.Serialize("Position"));
    element.Add(new XElement("TexturePath", data.Texture.Name));
    element.Add(data.Color.Serialize("Color"));
    element.Add(data.Source?.Serialize("Source"));
    element.Add(data.Origin.Serialize("Origin"));
    element.Add(new XElement("Rotation", data.Rotation));

    // Serialize children
    if (data.Children.Count > 0)
    {
      var childrenElement = new XElement("Children");
      foreach (var child in data.Children)
      {
        var childData = child.Serialize();
        if (childData != null)
        {
          childrenElement.Add(childData);
        }
      }
      element.Add(childrenElement);
    }
    return element;
  }

  public static void Deserialize(XElement data, Sprite sprite)
  {
    var texturePath = data.Element("TexturePath")?.Value;
    ArgumentNullException.ThrowIfNull(texturePath, "TexturePath cannot be null");
    if (!AssetsManager.Textures.TryGetValue(texturePath, out Texture2D? texture))
    {
      throw new InvalidOperationException($"Texture '{texturePath}' not found in AssetsManager");
    }
    sprite.Texture = texture;

    XElement? colorElement = data.Element("Color");
    if (colorElement != null)
    {
      sprite.Color = XmlValidationHelper.SafeParseColor(colorElement, sprite.Color);
    }
    XElement? sourceElement = data.Element("Source");
    if (sourceElement != null)
    {
      sprite.Source = XmlValidationHelper.SafeParseRectangle(sourceElement);
    }
    XElement? originElement = data.Element("Origin");
    if (originElement != null)
    {
      sprite.Origin = XmlValidationHelper.SafeParseVector2(originElement, sprite.Origin);
    }
    sprite.Rotation = XmlValidationHelper.SafeParseFloat(data.Element("Rotation")?.Value, sprite.Rotation);
  }
}