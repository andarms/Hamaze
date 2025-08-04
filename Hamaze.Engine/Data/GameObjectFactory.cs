using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;

namespace Hamaze.Engine.Data;


public static class GameObjectFactory
{
  private static readonly Dictionary<string, Func<GameObject>> gameObjectCreators = new()
  {
      { "GameObject", () => new GameObject() },
      { "Hitbox", () => new Hitbox() },
      { "Sprite", () => new Sprite() }
  };

  public static GameObject? CreateFromElement(XElement element)
  {
    string elementName = element.Name.LocalName;

    if (gameObjectCreators.TryGetValue(elementName, out var creator))
    {
      return creator();
    }

    return null;
  }

  public static void RegisterType<T>(string elementName, Func<T> creator) where T : GameObject
  {
    gameObjectCreators[elementName] = () => creator();
  }
}
