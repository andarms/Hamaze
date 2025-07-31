using System.Linq;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Systems.Traits;

public static class TraitGameObjectExtensions
{
  public static void AddTrait<T>(this GameObject gameObject, T trait) where T : Trait
  {
    gameObject.traits ??= [];
    gameObject.traits.Add(trait);
  }

  public static bool HasTrait<T>(this GameObject gameObject) where T : Trait
  {
    return gameObject.traits != null && gameObject.traits.Any(t => t is T);
  }

  public static T? GetTrait<T>(this GameObject gameObject) where T : Trait
  {
    return gameObject.traits?.FirstOrDefault(t => t is T) as T;
  }

  public static void RemoveTrait<T>(this GameObject gameObject) where T : Trait
  {
    gameObject.traits?.RemoveAll(t => t is T);
  }
}
