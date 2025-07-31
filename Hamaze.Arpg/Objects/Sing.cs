using System;
using Hamaze.Arpg.Content;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Sing : GameObject
{

  public bool IsOpen { get; private set; } = false;

  public Sing(string text)
  {
    Name = "Sing";
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
      Origin = new(8, 16),
      Color = Color.White,
      Source = new(128, 0, 16, 16)
    };
    AddChild(sprite);

    HasInteraction interactable = new() { Side = Directions.Down };
    interactable.OnInteraction.Connect(() =>
    {
      Console.WriteLine(text);
    });
    this.AddTrait(interactable);
    this.AddTrait(new IsSolid());
  }
}