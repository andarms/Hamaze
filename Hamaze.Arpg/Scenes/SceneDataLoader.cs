using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;

namespace Hamaze.Arpg.Scenes;

public static class SceneDataLoader
{
  public static IEnumerable<GameObject> LoadGameObjects()
  {
    string dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
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
    try
    {
      using StreamReader reader = new(dataPath);
      XElement root = XElement.Load(reader);
      var validationResult = XmlSchemaValidator.ValidateGameObject(root);
      validationResult.PrintToConsole();

      if (!validationResult.IsValid)
      {
        Console.WriteLine("XML validation failed. Attempting to load anyway...");
      }

      GameObject gameObject = new();
      gameObject.Deserialize(root);
      if (gameObject != null)
      {
        return gameObject;
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error loading XML file {dataPath}: {ex.Message}");
    }
    return null;
  }

}
