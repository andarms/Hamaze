using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Hamaze.Engine.Data;

public static class XmlSchemaValidator
{
  public static ValidationResult ValidateGameObject(XElement element)
  {
    var result = new ValidationResult();

    // Check required attributes
    if (element.Attribute("Name") == null)
    {
      result.AddWarning("GameObject is missing Name attribute");
    }

    // Validate Position element
    var positionElement = element.Element("Position");
    if (positionElement != null)
    {
      ValidateVector2(positionElement, "Position", result);
    }

    // Validate Collider element
    var colliderElement = element.Element("Collider");
    if (colliderElement != null)
    {
      ValidateCollider(colliderElement, result);
    }

    // Validate Children
    var childrenElement = element.Element("Children");
    if (childrenElement != null)
    {
      foreach (var child in childrenElement.Elements())
      {
        switch (child.Name.LocalName)
        {
          case "Sprite":
            ValidateSprite(child, result);
            break;
          case "GameObject":
            ValidateGameObject(child);
            break;
          default:
            result.AddWarning($"Unknown child element type: {child.Name.LocalName}");
            break;
        }
      }
    }

    return result;
  }

  private static void ValidateSprite(XElement element, ValidationResult result)
  {
    ValidateVector2(element.Element("Position"), "Sprite Position", result);

    var texturePathElement = element.Element("TexturePath");
    if (texturePathElement == null || string.IsNullOrEmpty(texturePathElement.Value))
    {
      result.AddError("Sprite is missing TexturePath element");
    }

    ValidateColor(element.Element("Color"), "Sprite Color", result);
    ValidateRectangle(element.Element("Source"), "Sprite Source", result);
    ValidateVector2(element.Element("Origin"), "Sprite Origin", result);

    var rotationElement = element.Element("Rotation");
    if (rotationElement != null && !XmlValidationHelper.TryParseFloat(rotationElement.Value, 0f, out _))
    {
      result.AddError($"Invalid Rotation value: {rotationElement.Value}");
    }
  }

  private static void ValidateCollider(XElement element, ValidationResult result)
  {
    ValidateVector2(element.Element("Offset"), "Collider Offset", result);
    ValidateVector2(element.Element("Size"), "Collider Size", result);
  }

  private static void ValidateVector2(XElement? element, string context, ValidationResult result)
  {
    if (element == null) return;

    if (!XmlValidationHelper.TryParseFloat(element.Attribute("X")?.Value, 0f, out _))
    {
      result.AddError($"Invalid X value in {context}: {element.Attribute("X")?.Value}");
    }

    if (!XmlValidationHelper.TryParseFloat(element.Attribute("Y")?.Value, 0f, out _))
    {
      result.AddError($"Invalid Y value in {context}: {element.Attribute("Y")?.Value}");
    }
  }

  private static void ValidateColor(XElement? element, string context, ValidationResult result)
  {
    if (element == null) return;

    string[] components = { "R", "G", "B", "A" };
    foreach (var component in components)
    {
      if (!XmlValidationHelper.TryParseByte(element.Attribute(component)?.Value, 255, out _))
      {
        result.AddError($"Invalid {component} value in {context}: {element.Attribute(component)?.Value}");
      }
    }
  }

  private static void ValidateRectangle(XElement? element, string context, ValidationResult result)
  {
    if (element == null) return;

    string[] components = { "X", "Y", "Width", "Height" };
    foreach (var component in components)
    {
      if (!XmlValidationHelper.TryParseInt(element.Attribute(component)?.Value, 0, out _))
      {
        result.AddError($"Invalid {component} value in {context}: {element.Attribute(component)?.Value}");
      }
    }
  }
}

public class ValidationResult
{
  public List<string> Errors { get; } = new();
  public List<string> Warnings { get; } = new();

  public bool IsValid => !Errors.Any();

  public void AddError(string message)
  {
    Errors.Add(message);
  }

  public void AddWarning(string message)
  {
    Warnings.Add(message);
  }

  public void PrintToConsole()
  {
    foreach (var error in Errors)
    {
      Console.WriteLine($"[ERROR] {error}");
    }

    foreach (var warning in Warnings)
    {
      Console.WriteLine($"[WARNING] {warning}");
    }
  }
}
