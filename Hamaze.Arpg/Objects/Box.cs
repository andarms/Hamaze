using System;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Box : GameObject
{
    public Box()
    {
        Name = "Box";
        Sprite sprite = new(AssetsManager.Textures["Sprites/TinyDungeon"])
        {
            Position = Position,
            Origin = new Vector2(8, 16),
            Color = Color.White,
            Source = new Rectangle(48, 96, 16, 16)
        };
        AddChild(sprite);

        Vector2 size = new Vector2(16) * Renderer.ScaleFactor;
        Collider = new(
            offset: new Vector2(0, 0),
            size: size
        );
        CollisionsManager.AddObject(this);

        this.AddTrait(new IsSolid());
    }
}
