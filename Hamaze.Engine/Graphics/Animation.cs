using System;
using System.Collections.Generic;
using Hamaze.Engine.Core;

namespace Hamaze.Engine.Graphics;

public class Animation(SpriteSheet spriteSheet, List<int> frames) : GameObject
{
  private int currentFrame = 0;
  private float timer = 0f;
  public float Speed { get; set; } = 100f; // milliseconds per frame

  public override void Update(float dt)
  {
    base.Update(dt);
    timer += dt;

    if (timer >= Speed / 1000f) // Convert milliseconds to seconds
    {
      currentFrame = (currentFrame + 1) % frames.Count;
      spriteSheet.SetFrame(frames[currentFrame]);
      timer = 0f;
    }
  }
}