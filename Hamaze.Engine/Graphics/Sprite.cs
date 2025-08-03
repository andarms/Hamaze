using System;
using System.Xml.Linq;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hamaze.Engine.Graphics;

public class Sprite : GameObject
{
  public Texture2D Texture { get; set; } = null!;
  public Color Color { get; set; } = Color.White;
  public Rectangle? Source { get; set; } = null;
  public Vector2 Origin { get; set; } = Vector2.Zero;
  public float Rotation { get; set; } = 0f;

  public Sprite(Texture2D texture)
  {
    Texture = texture;
    PersistenceData = new SpritePersistenceData(this);
  }

  public override void Draw(Renderer renderer)
  {
    renderer.DrawSprite(this);
    base.Draw(renderer);
  }
}


public class SpritePersistenceData(Sprite data) : IPersistenceData
{
  public Sprite Data { get; } = data;

  XElement IPersistenceData.Serialize()
  {
    var element = new XElement("Sprite");
    element.Add(Data.Position.Serialize("Position"));
    element.Add(new XElement("TexturePath", Data.Texture.Name));
    element.Add(Data.Color.Serialize("Color"));
    element.Add(Data.Source?.Serialize("Source"));
    element.Add(Data.Origin.Serialize("Origin"));
    element.Add(new XElement("Rotation", Data.Rotation));
    return element;
  }

  public void Deserialize(XElement data)
  {
    var texturePath = data.Element("TexturePath")?.Value;
    if (texturePath != null)
    {
      Data.Texture = AssetsManager.Textures[texturePath];
    }

    Data.Color.Deserialize(data.Element("Color")!);
    Data.Source?.Deserialize(data.Element("Source")!);
    Data.Origin = PersistenceDataExtensions.Deserialize(data.Element("Origin")!);
    Data.Rotation = float.Parse(data.Element("Rotation")?.Value ?? "0");
  }
}