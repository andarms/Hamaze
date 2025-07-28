using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Hamaze.Engine.Collisions;

public class Collider(Vector2 offset, Vector2 size)
{
    public Vector2 Offset { get; set; } = offset;
    public Vector2 Size { get; set; } = size;
}
