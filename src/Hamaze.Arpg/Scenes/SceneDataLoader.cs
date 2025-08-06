using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;

namespace Hamaze.Arpg.Scenes;

public static class SceneDataLoader
{

  public static void LoadResources(string dataDirectory)
  {
    if (!Directory.Exists(dataDirectory))
    {
      Console.WriteLine($"Warning: Data directory not found at {dataDirectory}");
      return;
    }

    string[] xresFiles = Directory.GetFiles(dataDirectory, "*.xres", SearchOption.AllDirectories);
    Console.WriteLine($"Found {xresFiles.Length} XRES file(s) in {dataDirectory}:");
    foreach (string xresFile in xresFiles)
    {
      Console.WriteLine($"  - {xresFile}");
    }
  }

  public static IEnumerable<GameObject> LoadGameObjects(string dataDirectory)
  {
    if (!Directory.Exists(dataDirectory))
    {
      Console.WriteLine($"Warning: Data directory not found at {dataDirectory}");
      yield break;
    }

    string[] xmlFiles = Directory.GetFiles(dataDirectory, "*.xml", SearchOption.AllDirectories);

    if (xmlFiles.Length == 0)
    {
      Console.WriteLine($"Warning: No XML files found in {dataDirectory}");
      yield break;
    }

    Console.WriteLine($"Found {xmlFiles.Length} XML file(s) in Data directory:");
    foreach (string xmlFile in xmlFiles)
    {
      string relativePath = Path.GetRelativePath(dataDirectory, xmlFile);
      Console.WriteLine($"  - {relativePath}");
      GameObject? obj = LoadGameObjectFromXml(xmlFile);
      if (obj != null)
      {
        yield return obj;
      }
      else
      {
        Console.WriteLine($"Failed to load GameObject from {relativePath}");
      }
    }
  }

  private static GameObject? LoadGameObjectFromXml(string dataPath)
  {
    using StreamReader reader = new(dataPath);
    XElement root = XElement.Load(reader);
    GameObject gameObject = GameObjectFactory.CreateFromElement(root) ?? new GameObject();
    gameObject.Deserialize(root);
    if (gameObject != null)
    {
      return gameObject;
    }

    return null;
  }

}
