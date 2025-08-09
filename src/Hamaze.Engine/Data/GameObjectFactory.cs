using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Inventory;

namespace Hamaze.Engine.Data;


public static class GameObjectFactory
{
  private static readonly Dictionary<string, Func<XElement, GameObject>> gameObjectCreators = new()
  {
    { "GameObject", (element) => new GameObject() },
    { "CollectableItem", (element) => new CollectableItem() },
    { "Hitbox", (element) => new Hitbox() },
    { "Sprite", (element) => new Sprite() }
  };

  public static GameObject? CreateFromElement(XElement element)
  {
    string elementName = element.Attribute("type")?.Value ?? element.Name.LocalName;

    if (gameObjectCreators.TryGetValue(elementName, out var creator))
    {
      return creator(element);
    }

    return null;
  }

  public static void RegisterType<T>(string elementName, Func<XElement, T> creator) where T : GameObject
  {
    gameObjectCreators[elementName] = creator;
  }
}
