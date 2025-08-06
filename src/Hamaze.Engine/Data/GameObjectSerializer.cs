using System;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Systems.Traits;

namespace Hamaze.Engine.Data;

public static class GameObjectSerializer
{
  public static XElement Serialize(GameObject data)
  {
    XElement element = new(data.GetType().Name);
    element.SetAttributeValue("Name", data.Name);
    element.Add(data.Position.Serialize("Position"));
    if (data.Collider != null)
    {
      element.Add(data.Collider.Serialize("Collider"));
    }

    if (data.Traits.Count > 0)
    {
      XElement traitsElement = new("Traits");
      element.Add(traitsElement);
      foreach (var (_, trait) in data.Traits)
      {
        XElement traitElement = trait.Serialize();
        traitsElement.Add(traitElement);
      }
    }
    return element;
  }

  public static void Deserialize(GameObject obj, XElement saveData)
  {
    ArgumentNullException.ThrowIfNull(saveData, saveData.GetType().Name);

    obj.Name = saveData.Attribute("Name")?.Value ?? "Game Object";
    XElement? positionData = saveData.Element("Position");
    if (positionData != null)
    {
      obj.Position = XmlValidationHelper.SafeParseVector2(positionData, obj.Position);
    }

    XElement? colliderData = saveData.Element("Collider");
    if (colliderData != null)
    {
      Collider collider = new();
      collider.Deserialize(colliderData);
      obj.SetCollider(collider);
    }

    XElement? childrenElement = saveData.Element("Children");
    if (childrenElement != null)
    {
      foreach (var childElement in childrenElement.Elements())
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
    }

    XElement? traitsElement = saveData.Element("Traits");
    if (traitsElement != null)
    {
      foreach (var traitData in traitsElement.Elements())
      {
        Trait? trait = TraitFactory.CreateFromType(traitData);
        if (trait != null)
        {
          obj.AddTrait(trait);
        }
        else
        {
          Console.WriteLine($"Warning: Unknown trait element type '{traitData.Name.LocalName}' skipped");
        }
      }
    }
  }
}
