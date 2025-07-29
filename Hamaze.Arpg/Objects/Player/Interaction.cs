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
  Vector2 size = new(32);
  public Interaction(Player player)
  {
    this.player = player;
    Name = "Interaction";
    Collider = new Collider(
      offset: new Vector2(0, 0),
      size: size
    )
    {
      DebugColor = Color.Green
    };
    CollisionsManager.AddObject(this);
  }

  public override void Update(float dt)
  {
    base.Update(dt);

    // Place the interaction area at half of the player's size in the facing direction
    var playerCenter = player.Collider.Size * 0.5f;
    var offset = player.FacingDirection * (player.Collider.Size * 0.5f);
    Position = playerCenter + offset - size * 0.5f;

    if (InputManager.IsActionJustPressed("confirm"))
    {
      var hit = CollisionsManager.GetPotentialCollisions(this).FirstOrDefault(x => x.Traits.Has<Interactable>());
      hit?.Traits.Get<Interactable>()?.OnInteraction.Emit();
    }
  }
}