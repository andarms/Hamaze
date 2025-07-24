
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

/// <summary>
/// AABB collider component for 2D physics objects
/// </summary>
public class Collider : GameObject
{
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Width of the collider
    /// </summary>
    public int Width
    {
        get => Bounds.Width;
        set => Bounds = new Rectangle(Bounds.X, Bounds.Y, value, Bounds.Height);
    }

    /// <summary>
    /// Height of the collider
    /// </summary>
    public int Height
    {
        get => Bounds.Height;
        set => Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, value);
    }

    /// <summary>
    /// Offset from the parent object's position
    /// </summary>
    public Vector2 Offset
    {
        get => new Vector2(Bounds.X, Bounds.Y);
        set => Bounds = new Rectangle((int)value.X, (int)value.Y, Bounds.Width, Bounds.Height);
    }

    public Collider(int width, int height, Vector2 offset = default)
    {
        Bounds = new Rectangle((int)offset.X, (int)offset.Y, width, height);
    }

    public Collider(Rectangle bounds)
    {
        Bounds = bounds;
    }

    /// <summary>
    /// Create a collider that matches the given rectangle size
    /// </summary>
    public static Collider FromSize(int width, int height) => new(width, height);

    /// <summary>
    /// Create a collider with an offset from the parent position
    /// </summary>
    public static Collider FromSizeAndOffset(int width, int height, Vector2 offset) => new(width, height, offset);
}