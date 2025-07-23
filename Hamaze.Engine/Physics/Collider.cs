
using Hamaze.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Physics;

public class Collider(Rectangle bounds) : GameObject
{
    public Rectangle Bounds { get; set; } = bounds;
}