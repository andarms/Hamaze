using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Core;

public static class AssetsManager
{
  public static Dictionary<string, Texture2D> Textures { get; } = [];

  public static Dictionary<string, SpriteFont> Fonts { get; } = [];

  public static void RequestTexture(string path)
  {
    if (!Textures.ContainsKey(path))
    {
      Textures[path] = null!;
    }
  }

  public static void LoadContent(ContentManager content)
  {
    foreach (var texture in Textures.Keys)
    {
      Textures[texture] = content.Load<Texture2D>(texture);
    }

    foreach (var font in Fonts.Keys)
    {
      Fonts[font] = content.Load<SpriteFont>(font);
    }
  }


  public static void RequestFont(string path)
  {
    if (!Fonts.ContainsKey(path))
    {
      Fonts[path] = null!;
    }
  }


  public static void UnloadContent()
  {
    Textures.Clear();
    Fonts.Clear();
  }
}
