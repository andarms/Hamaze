using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Data;

public abstract class Resource : ISaveable
{
  public virtual void Deserialize(XElement data)
  {
    foreach (var member in GetSavedMembers())
    {
      if (member is not PropertyInfo property)
      {
        continue;
      }

      // Prefer direct element by name, else property-tagged generic wrapper
      XElement? element = data.Element(property.Name);
      if (element == null)
      {
        element = data.Elements().FirstOrDefault(e => (string?)e.Attribute("property") == property.Name);
      }
      if (element == null)
      {
        throw new InvalidOperationException($"Missing element: {property.Name}");
      }
      var value = MemberSerializer.Deserialize(element, property.PropertyType);
      property.SetValue(this, value);
    }
  }

  public virtual XElement Serialize()
  {
    XElement root = new("Resource");
    root.SetAttributeValue("Type", GetType().Name);

    foreach (var member in GetSavedMembers())
    {
      if (member is not PropertyInfo property)
      {
        continue;
      }

      var value = property.GetValue(this);
      var e = MemberSerializer.Serialize(property, value);
      if (e.Name.LocalName == "GameObject")
      {
        e.SetAttributeValue("property", property.Name);
      }
      root.Add(e);
    }

    return root;
  }

  public IEnumerable<MemberInfo> GetSavedMembers()
  {
    return GetType()
    .GetMembers(BindingFlags.Instance | BindingFlags.Public)
    .Where(m => (m is PropertyInfo or FieldInfo) && m.GetCustomAttribute<SaveAttribute>() != null);
  }
}
