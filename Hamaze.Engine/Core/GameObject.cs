using System;
using System.Collections.Generic;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Core;

public class GameObject : IDisposable
{
  public string Name { get; set; } = "Game Object";
  public Vector2 Position { get; set; } = Vector2.Zero;
  public GameObject? Parent { get; set; } = null;
  public List<GameObject> Children { get; } = [];
  public TraitCollection Traits { get; } = [];

  public Collider? Collider { get; set; }
  public Rectangle Bounds => GetColliderBounds();
  public List<GameObject> Collisions { get; } = [];

  private Rectangle GetColliderBounds()
  {
    if (Collider == null) return Rectangle.Empty;
    var offset = Collider.Offset;
    var size = Collider.Size;
    return new Rectangle((int)(Position.X + offset.X), (int)(Position.Y + offset.Y), (int)size.X, (int)size.Y);
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
  }

  public virtual void Dispose()
  {
    Children.ForEach(c => c.Dispose());
    Children.Clear();
  }
  #endregion
}