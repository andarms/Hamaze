using Hamaze.Arpg.Content;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Ghost;

public class Ghost : GameObject
{

    public Ghost()
    {
        Sprite sprite = new(AssetsManager.TinyDungeon)
        {
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(16, 160, 16, 16)
        };
        AddChild(sprite);

        

        // WobbleMovementAnimation wobbleAnimation = new(sprite, movement);
        // AddChild(wobbleAnimation);
    }
}