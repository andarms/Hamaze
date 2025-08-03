using Microsoft.Xna.Framework;
using Hamaze.Engine.Core.Scenes;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;

namespace Hamaze.Engine.Systems.Inventory;

public class InventoryScene : Scene
{
  public override Color BackgroundColor => Color.CornflowerBlue;

  public InventoryScene()
  {
    NinePatchSprite ninePatchSprite = new(AssetsManager.Textures["UI/Inventory/Window"], 7)
    {
      Position = new Vector2(240, 204),
      Size = new Vector2(800, 400)
    };
    AddChild(ninePatchSprite, UIOverlayLayerName);
  }

  public override void Initialize()
  {
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.IsActionJustPressed("ToggleInventory"))
    {
      SceneManager.PopScene();
    }
  }

  public override void Draw(Renderer renderer)
  {
    base.Draw(renderer);
  }
}