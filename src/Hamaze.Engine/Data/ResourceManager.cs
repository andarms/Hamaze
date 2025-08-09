using System;
using System.Collections.Generic;
using System.Linq;

namespace Hamaze.Engine.Data;

public static class ResourceManager
{
  private static readonly Dictionary<string, Func<Resource>> factories = [];

  public static void RegisterResourceType(string type, Func<Resource> factory)
  {
    ArgumentNullException.ThrowIfNull(type, nameof(type));
    ArgumentNullException.ThrowIfNull(factory, nameof(factory));
    factories[type] = factory;
  }

  public static Resource Create(string type)
  {
    if (factories.TryGetValue(type, out var factory))
    {
      return factory();
    }

    throw new ArgumentException("Invalid resource type", nameof(type));
  }


  public static bool IsResourceTypeRegistered(string type)
  {
    var name = type.Contains('.') ? type.Split('.').Last() : type;
    return factories.ContainsKey(name);
  }


  public static void EnsureAllResourcesAreRegistered()
  {
    var allResourceTypes = AppDomain.CurrentDomain.GetAssemblies()
      .SelectMany(a => a.GetTypes())
      .Where(t => t.IsSubclassOf(typeof(Resource)) && !t.IsAbstract)
      .ToList();

    foreach (var type in allResourceTypes)
    {
      if (!factories.ContainsKey(type.Name))
      {
        throw new InvalidOperationException($"Resource type '{type.Name}' is not registered.");
      }
    }
  }
}

