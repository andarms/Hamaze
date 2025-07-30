using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Arpg.Content;

public static class AssetsManager
{
  public static Texture2D TinyDungeon { get; set; }
  public static Texture2D Boy { get; set; }
  public static Texture2D NinjaGreen { get; set; }

  public static Texture2D TilesetElement { get; set; }

  public static void LoadContent(ContentManager Content)
  {
    TinyDungeon = Content.Load<Texture2D>("Sprites/TinyDungeon");
    NinjaGreen = Content.Load<Texture2D>("Sprites/NinjaGreen/SpriteSheet");
    Boy = Content.Load<Texture2D>("Sprites/Boy/SpriteSheet");
    TilesetElement = Content.Load<Texture2D>("Tilesets/TilesetElement");
  }

  public static void UnloadContent()
  {
    TinyDungeon?.Dispose();
    TinyDungeon = null;

    Boy?.Dispose();
    Boy = null;

    NinjaGreen?.Dispose();
    NinjaGreen = null;

    TilesetElement?.Dispose();
    TilesetElement = null;
  }


}