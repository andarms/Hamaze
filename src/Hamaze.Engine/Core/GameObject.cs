using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Data;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Core;

public class GameObject : IDisposable
{
  public string Name { get; set; } = "Game Object";
  public Vector2 Position { get; set; } = Vector2.Zero;
  public GameObject? Parent { get; set; } = null;
  public List<GameObject> Children { get; } = [];
  private readonly Dictionary<Type, Trait> traits = [];
  internal Dictionary<Type, Trait> Traits => traits;
  public Collider? Collider { get; set; }

  public Rectangle Bounds => GetColliderBounds();
  public List<GameObject> Collisions { get; } = [];

  private Rectangle GetColliderBounds()
  {
    if (Collider == null) return Rectangle.Empty;
    var offset = Collider.Offset;
    var size = Collider.Size;
    return new Rectangle((int)(GlobalPosition.X + offset.X), (int)(GlobalPosition.Y + offset.Y), (int)size.X, (int)size.Y);
  }

  public Vector2 GlobalPosition
  {
    get => GetGlobalPosition();
    set => SetGlobalPosition(value);
  }

  private Vector2 GetGlobalPosition()
  {
    return Parent == null ? Position : Parent.GlobalPosition + Position;
  }

  private void SetGlobalPosition(Vector2 value)
  {
    Position = Parent == null ? value : value - Parent.GlobalPosition;
  }

  public virtual XElement? Serialize()
  {
    XElement root = GameObjectSerializer.Serialize(this);
    var a = GetSavedMembers();
    foreach (var member in a)
    {
      if (member is PropertyInfo property)
      {
        XElement m = MemberSerializer.Serialize(property, property.GetValue(this));
        root.Add(m);
      }
    }
    // If a CanBeCollected trait exists but has no Item, and this object has an Item property,
    // connect them in-memory after load; serialization remains single-source of truth.
    if (Children.Count > 0)
    {
      var childrenElement = new XElement("Children");
      foreach (var child in Children)
      {
        var childData = child.Serialize();
        if (childData != null)
        {
          childrenElement.Add(childData);
        }
      }
      root.Add(childrenElement);
    }
    return root;
  }

  public virtual void Deserialize(XElement data)
  {
    GameObjectSerializer.Deserialize(this, data);
    foreach (var member in GetSavedMembers())
    {
      if (member is not PropertyInfo property)
      {
        continue;
      }

      // Support property-tagged resources and game objects where the element might be a generic wrapper
      var direct = data.Element(property.Name);
      XElement? selected = direct;
      if (selected == null)
      {
        // Try find by property attribute
        selected = data.Elements()
          .FirstOrDefault(e => (string?)e.Attribute("property") == property.Name);
      }
      var value = MemberSerializer.Deserialize(selected ?? throw new InvalidOperationException($"Missing element: {property.Name}"), property.PropertyType);
      property.SetValue(this, value);
    }

    // Wire trait Item from object-level Item if applicable
    var canBeCollected = this.Trait<Systems.Inventory.CanBeCollected>();
    var itemProp = GetType().GetProperty("Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (canBeCollected != null && itemProp != null)
    {
      var itemVal = itemProp.GetValue(this) as Systems.Inventory.Item;
      if (itemVal != null)
      {
        canBeCollected.Item = itemVal;
      }
    }
  }

  #region Children Management
  public void AddChild(GameObject child)
  {
    child.Parent = this;
    Children.Add(child);
  }

  public void RemoveChild(GameObject child)
  {
    if (Children.Remove(child))
    {
      child.Parent = null;
    }
  }


  public void SetCollider(Collider collider)
  {
    Collider = collider;
    CollisionsManager.AddObject(this);
  }
  #endregion

  #region  Lifecycle Methods
  public virtual void Initialize()
  {
    Children.ForEach(c => c.Initialize());
  }

  public virtual void Update(float dt)
  {
    Children.ForEach(c => c.Update(dt));
  }

  public virtual void Draw(Renderer renderer)
  {
    Children.ForEach(c => c.Draw(renderer));
    Debug(renderer);
  }

  private void Debug(Renderer renderer)
  {
    if (Collider == null) return;
    renderer.Shapes.DrawRectangle(
      xy: GlobalPosition + Collider.Offset,
      size: Collider.Size,
      c1: Collider.DebugColor,
      c2: Collider.DebugOutlineColor,
      thickness: 2
    );

  }

  public virtual void Dispose()
  {
    Children.ForEach(c => c.Dispose());
    Children.Clear();
    traits.Clear();

    Parent = null;
    Collider = null;
  }
  #endregion

  public IEnumerable<MemberInfo> GetSavedMembers() => GetType()
    .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
    .Where(m => (m is PropertyInfo or FieldInfo) && m.GetCustomAttribute<SaveAttribute>() != null);

}
