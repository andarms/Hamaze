using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

      if (ResourceManager.IsResourceTypeRegistered(property.PropertyType.Name))
      {
        XElement ee = data.Element("Resource") ?? throw new InvalidOperationException($"Missing element: {property.Name}");
        var vv = MemberSerializer.Deserialize(ee, property.PropertyType);
        property.SetValue(this, vv);
        continue;
      }

      XElement element = data.Element(property.Name) ?? throw new InvalidOperationException($"Missing element: {property.Name}");
      var value = MemberSerializer.Deserialize(element, property.PropertyType);
      property.SetValue(this, value);
    }
  }

  public virtual XElement Serialize()
  {
    XElement element = new("Resource");
    element.SetAttributeValue("Type", GetType().Name);

    foreach (var member in GetSavedMembers())
    {
      if (member is not PropertyInfo property)
      {
        continue;
      }

      var value = property.GetValue(this);
      element.Add(MemberSerializer.Serialize(property, value));
    }

    return element;
  }

  public IEnumerable<MemberInfo> GetSavedMembers()
  {
    return GetType()
    .GetMembers(BindingFlags.Instance | BindingFlags.Public)
    .Where(m => (m is PropertyInfo or FieldInfo) && m.GetCustomAttribute<SaveAttribute>() != null);
  }
}
