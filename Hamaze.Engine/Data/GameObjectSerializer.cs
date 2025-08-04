using System;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Data;

public static class GameObjectSerializer
{
  public static XElement Serialize(GameObject data)
  {
    XElement element = new("GameObject");
    element.SetAttributeValue("Name", data.Name);
    element.Add(data.Position.Serialize("Position"));
    if (data.Collider != null)
    {
      element.Add(data.Collider.Serialize("Collider"));
    }
    return element;
  }

  public static void Deserialize(GameObject obj, XElement saveData)
  {
    ArgumentNullException.ThrowIfNull(saveData);

    obj.Name = saveData.Attribute("Name")?.Value ?? "Game Object";

    XElement? positionData = saveData.Element("Position");
    if (positionData != null)
    {
      obj.Position = XmlValidationHelper.SafeParseVector2(positionData, obj.Position);
    }

    XElement? colliderData = saveData.Element("Collider");
    if (colliderData != null)
    {
      obj.Collider = new Collider();
      obj.Collider = obj.Collider.Deserialize(colliderData);
    }

    // Deserialize children
    XElement? childrenElement = saveData.Element("Children");
    if (childrenElement != null)
    {
      foreach (var childElement in childrenElement.Elements())
      {
        try
        {
          GameObject? child = GameObjectFactory.CreateFromElement(childElement);
          if (child != null)
          {
            child.Deserialize(childElement);
            obj.AddChild(child);
          }
          else
          {
            Console.WriteLine($"Warning: Unknown child element type '{childElement.Name.LocalName}' skipped");
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error deserializing child element '{childElement.Name.LocalName}': {ex.Message}");
        }
      }
    }
  }
}
