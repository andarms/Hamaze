using System;
using Hamaze.Arpg.Content;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Chest : GameObject
{
  public Rectangle OpenSource { get; private set; } = new Rectangle(112, 0, 16, 16);
  public Rectangle ClosedSource { get; private set; } = new Rectangle(96, 0, 16, 16);

  public bool IsOpen { get; private set; } = false;

  public Chest()
  {
    Name = "Chest";
    Collider = new(
      offset: new Vector2(0, 32),
      size: new Vector2(64, 32)
    )
    {
      DebugColor = Color.Blue
    };
    CollisionsManager.AddObject(this);



    Sprite sprite = new(AssetsManager.TilesetElement)
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = ClosedSource
    };
    AddChild(sprite);

    Interactable interactable = new() { Side = Directions.Down };
    interactable.OnInteraction.Connect(() =>
    {
      IsOpen = !IsOpen;
      sprite.Source = IsOpen ? OpenSource : ClosedSource;
      Traits.Remove<Interactable>();
    });
    Traits.Add(interactable);
    Traits.Add(new Solid());
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    // Update logic for the chess object can be added here
  }
}