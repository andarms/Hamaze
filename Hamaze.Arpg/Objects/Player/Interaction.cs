using System;
using System.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Input;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects.Player;

public class Interaction : GameObject
{
  readonly Player player;
  Vector2 size = new(64);
  public Interaction(Player player)
  {
    this.player = player;
    Name = "Interaction";
    Collider = new Collider(
      offset: new Vector2(0, 0),
      size: new Vector2(64)
    )
    {
      DebugColor = Color.Green
    };
    CollisionsManager.AddObject(this);
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    Position = player.FacingDirection * size;

    if (InputManager.IsActionJustPressed("confirm"))
    {
      var hit = CollisionsManager.GetPotentialCollisions(this).FirstOrDefault(x => x.Traits.Has<Interactable>());
      hit?.Traits.Get<Interactable>()?.OnInteraction.Emit();
    }
  }
}