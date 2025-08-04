using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;

namespace Hamaze.Engine.Data;

public interface IGameObjectFactory
{
  GameObject? CreateFromElement(XElement element);
}

public class DefaultGameObjectFactory : IGameObjectFactory
{
  private readonly Dictionary<string, Func<GameObject>> gameObjectCreators = new()
  {
      { "GameObject", () => new GameObject() },
      { "Sprite", () => new Sprite() }
  };

  public GameObject? CreateFromElement(XElement element)
  {
    string elementName = element.Name.LocalName;

    if (gameObjectCreators.TryGetValue(elementName, out var creator))
    {
      return creator();
    }

    return null;
  }

  public void RegisterType<T>(string elementName, Func<T> creator) where T : GameObject
  {
    gameObjectCreators[elementName] = () => creator();
  }
}
