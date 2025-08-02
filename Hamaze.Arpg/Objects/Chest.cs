using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Systems.Traits;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class Chest : GameObject
{
  public Rectangle OpenSource { get; private set; } = new Rectangle(112, 0, 16, 16);
  public Rectangle ClosedSource { get; private set; } = new Rectangle(96, 0, 16, 16);

  public bool IsOpen { get; private set; } = false;

  private readonly Sprite sprite;

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



    sprite = new(AssetsManager.Textures["Tilesets/TilesetElement"])
    {
      Position = Position,
      Origin = new Vector2(8, 16),
      Color = Color.White,
      Source = ClosedSource
    };
    AddChild(sprite);

    HasInteraction interactable = new() { Side = Directions.Down };
    interactable.OnInteraction.Connect(ToggleChestState);
    this.AddTrait(interactable);

    this.AddTrait(new IsSolid());
  }

  private void ToggleChestState()
  {
    IsOpen = !IsOpen;
    sprite.Source = IsOpen ? OpenSource : ClosedSource;

  }
}