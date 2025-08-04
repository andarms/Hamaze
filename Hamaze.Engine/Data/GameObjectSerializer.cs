using System;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Data;

public class GameObjectSerializer(GameObject data) : ISerializableData
{
  private static readonly IGameObjectFactory Factory = new DefaultGameObjectFactory();

  public XElement Serialize()
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

  public void Deserialize(XElement saveData)
  {
    ArgumentNullException.ThrowIfNull(saveData);

    try
    {
      data.Name = saveData.Attribute("Name")?.Value ?? "Game Object";

      XElement? positionData = saveData.Element("Position");
      if (positionData != null)
      {
        data.Position = XmlValidationHelper.SafeParseVector2(positionData, data.Position);
      }

      XElement? colliderData = saveData.Element("Collider");
      if (colliderData != null)
      {
        data.Collider = new Collider();
        data.Collider = data.Collider.Deserialize(colliderData);
      }

      // Deserialize children
      XElement? childrenElement = saveData.Element("Children");
      if (childrenElement != null)
      {
        foreach (var childElement in childrenElement.Elements())
        {
          try
          {
            GameObject? child = Factory.CreateFromElement(childElement);
            if (child != null)
            {
              child.Deserialize(childElement);
              data.AddChild(child);
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
    catch (Exception ex)
    {
      Console.WriteLine($"Error deserializing GameObject '{data.Name}': {ex.Message}");
    }
  }

}
