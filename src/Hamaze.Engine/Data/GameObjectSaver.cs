using System;
using System.IO;
using System.Xml.Linq;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Data;

public static class GameObjectSaver
{
  public static bool SaveToFile(GameObject gameObject, string filePath)
  {
    try
    {
      var serializedData = gameObject.Serialize();
      if (serializedData == null)
      {
        Console.WriteLine("Failed to serialize GameObject - no data returned");
        return false;
      }

      // Ensure directory exists
      var directory = Path.GetDirectoryName(filePath);
      if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      // Create XML document with formatting
      var document = new XDocument(
          new XDeclaration("1.0", "utf-8", "yes"),
          serializedData
      );

      // Save with proper indentation
      document.Save(filePath, SaveOptions.None);
      Console.WriteLine($"Successfully saved GameObject '{gameObject.Name}' to {filePath}");
      return true;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error saving GameObject to {filePath}: {ex.Message}");
      return false;
    }
  }

  public static bool SaveToFileFormatted(GameObject gameObject, string filePath)
  {
    try
    {
      var serializedData = gameObject.Serialize();
      if (serializedData == null)
      {
        Console.WriteLine("Failed to serialize GameObject - no data returned");
        return false;
      }

      // Ensure directory exists
      var directory = Path.GetDirectoryName(filePath);
      if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      // Create formatted XML string
      var document = new XDocument(
          new XDeclaration("1.0", "utf-8", "yes"),
          serializedData
      );

      // Save with custom formatting for better readability
      using var writer = new StreamWriter(filePath);
      document.Save(writer, SaveOptions.None);

      Console.WriteLine($"Successfully saved GameObject '{gameObject.Name}' to {filePath}");
      return true;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error saving GameObject to {filePath}: {ex.Message}");
      return false;
    }
  }
}
