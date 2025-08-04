using System;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;

namespace Hamaze.Engine.Graphics;

public class SpriteSerializer(Sprite data) : ISerializableData
{
  public Sprite Data { get; } = data;

  XElement ISerializableData.Serialize()
  {
    var element = new XElement("Sprite");
    element.Add(Data.Position.Serialize("Position"));
    element.Add(new XElement("TexturePath", Data.Texture.Name));
    element.Add(Data.Color.Serialize("Color"));
    element.Add(Data.Source?.Serialize("Source"));
    element.Add(Data.Origin.Serialize("Origin"));
    element.Add(new XElement("Rotation", Data.Rotation));
    return element;
  }

  public void Deserialize(XElement data)
  {
    try
    {
      // Deserialize position from data
      XElement? positionElement = data.Element("Position");
      if (positionElement != null)
      {
        Data.Position = XmlValidationHelper.SafeParseVector2(positionElement);
      }

      // Deserialize texture
      var texturePath = data.Element("TexturePath")?.Value;
      if (!string.IsNullOrEmpty(texturePath) && AssetsManager.Textures.ContainsKey(texturePath))
      {
        Data.Texture = AssetsManager.Textures[texturePath];
      }
      else if (!string.IsNullOrEmpty(texturePath))
      {
        // Log warning about missing texture
        Console.WriteLine($"Warning: Texture '{texturePath}' not found in AssetsManager");
      }

      // Deserialize color
      XElement? colorElement = data.Element("Color");
      if (colorElement != null)
      {
        Data.Color = XmlValidationHelper.SafeParseColor(colorElement, Data.Color);
      }

      // Deserialize source rectangle
      XElement? sourceElement = data.Element("Source");
      if (sourceElement != null)
      {
        Data.Source = XmlValidationHelper.SafeParseRectangle(sourceElement);
      }

      // Deserialize origin
      XElement? originElement = data.Element("Origin");
      if (originElement != null)
      {
        Data.Origin = XmlValidationHelper.SafeParseVector2(originElement, Data.Origin);
      }

      // Deserialize rotation
      Data.Rotation = XmlValidationHelper.SafeParseFloat(data.Element("Rotation")?.Value, Data.Rotation);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error deserializing Sprite: {ex.Message}");
      // Keep default values on error
    }
  }
}