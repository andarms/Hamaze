using Hamaze.Arpg.Content;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Box : SolidObject
{
    public Box()
    {
        Name = "Box";
        Sprite sprite = new(AssetsManager.TinyDungeon)
        {
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(48, 96, 16, 16)
        };
        AddChild(sprite);

        Vector2 size = new Vector2(16, 16) * Renderer.ScaleFactor;
        Collider collider = new(new(0, 0, (int)size.X, (int)size.Y));
        AddChild(collider);
    }
}
