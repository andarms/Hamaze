using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Arpg.Content;

public static class AssetsManager
{
  public static Texture2D TinyDungeon { get; set; }
  public static Texture2D Boy { get; set; }

  public static void LoadContent(ContentManager Content)
  {
    TinyDungeon = Content.Load<Texture2D>("Sprites/TinyDungeon");
    Boy = Content.Load<Texture2D>("Sprites/NinjaGreen/SpriteSheet");
  }

  public static void UnloadContent()
  {
    TinyDungeon?.Dispose();
    TinyDungeon = null;

    Boy?.Dispose();
    Boy = null;
  }


}