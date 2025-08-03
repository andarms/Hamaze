using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Data;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Core;

public class GameObjectSerializer(GameObject data) : IPersistenceData
{
  public XElement Serialize()
  {
    XElement element = new("GameObject");
    element.SetAttributeValue("Name", data.Name);
    element.Add(data.Position.Serialize("Position"));
    if (data.Collider != null)
    {
      element.Add(data.Collider.Serialize("Collider"));
    }
    return element;
  }

  public void Deserialize(XElement saveData)
  {
    ArgumentNullException.ThrowIfNull(saveData);

    data.Name = saveData.Attribute("Name")?.Value ?? "Game Object";
    XElement? positionData = saveData.Element("Position");
    if (positionData != null)
    {
      data.Position = PersistenceDataExtensions.Deserialize(positionData);
    }

    XElement? colliderData = saveData.Element("Collider");
    if (colliderData != null)
    {
      data.Collider = new Collider();
      data.Collider = data.Collider.Deserialize(colliderData);
    }

    // foreach (var childElement in saveData.Element("Children")?.Elements() ?? [])
    // {
    //   GameObject child = new GameObject();
    //   child.Deserialize(childElement);
    //   data.AddChild(child);
    // }
  }

}

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

  public GameObject()
  {
    PersistenceData = new GameObjectSerializer(this);
  }

  public IPersistenceData? PersistenceData { get; set; } = null;

  public XElement? Serialize()
  {
    XElement? root = PersistenceData?.Serialize();
    if (root != null && Children.Count > 0)
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

  public void Deserialize(XElement data)
  {
    PersistenceData?.Deserialize(data);
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
}