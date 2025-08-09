using System;
using System.Reflection;
using System.Xml.Linq;
using Hamaze.Engine.Components.Attack;
using Hamaze.Engine.Data;
using Hamaze.Engine.Systems.Inventory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Core;

public static class MemberSerializer
{
  public static XElement Serialize(MemberInfo member, object? value)
  {
    if (value is IDamageCalculator damageable)
    {
      return DamageSerializer.Serialize(damageable, member.Name);
    }

    if (value is Texture2D texture)
    {
      return XmlValidationHelper.SerializeTexture(texture, member.Name);
    }

    if (value is Vector2 vector)
    {
      return vector.Serialize(member.Name);
    }

    if (value is Rectangle rectangle)
    {
      return rectangle.Serialize(member.Name);
    }

    if (value is Color color)
    {
      return color.Serialize(member.Name);
    }

    if (value is ISaveable saveable)
    {
      return saveable.Serialize();
    }

    return new XElement(member.Name, value);
  }


  public static object? Deserialize(XElement element, Type type)
  {
    if (ResourceManager.IsResourceTypeRegistered(type.Name))
    {
      return ResourceManager.Create(type.Name);
    }
    return type switch
    {
      _ when type == typeof(int) => int.Parse(element.Value),
      _ when type == typeof(float) => float.Parse(element.Value),
      _ when type == typeof(string) => element.Value,
      _ when type == typeof(bool) => bool.Parse(element.Value),
      _ when type == typeof(Vector2) => XmlValidationHelper.SafeParseVector2(element, Vector2.Zero),
      _ when type == typeof(Rectangle) => XmlValidationHelper.SafeParseRectangle(element, Rectangle.Empty),
      _ when type == typeof(Rectangle?) => XmlValidationHelper.SafeParseRectangle(element, Rectangle.Empty),
      _ when type == typeof(Color) => XmlValidationHelper.SafeParseColor(element, Color.White),
      _ when type == typeof(IDamageCalculator) => DamageSerializer.Deserialize(element),
      _ when type == typeof(Texture2D) => XmlValidationHelper.SafeParseTexture(element),
      // _ when type == typeof(Item) => Item.FromElement(element),
      _ when type == typeof(ISaveable) => Activator.CreateInstance(type),
      _ => throw new NotSupportedException($"Deserialization for type {type} is not supported.")
    };
  }
}