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
    UpdatePosition();
    if (InputManager.IsActionJustPressed("confirm"))
    {
      ProcessInteraction();
    }
  }

  private void UpdatePosition()
  {
    var playerCenter = player.Collider.Offset + player.Collider.Size * 0.5f;
    // Calculate the offset in the facing direction, placing the interaction area just outside the player
    var facingOffset = player.FacingDirection.ToVector2() * (player.Collider.Size.Y * 0.5f + size.Y * 0.5f);
    Position = playerCenter + facingOffset - size * 0.5f;
  }

  private void ProcessInteraction()
  {
    var hit = CollisionsManager.GetPotentialCollisions(this).FirstOrDefault(x => x.Traits.Has<Interactable>());
    if (hit == null) return;

    Interactable interactable = hit.Traits.Get<Interactable>();
    if (interactable == null) return;

    // Check if the interactable has a side and if it matches the player's facing direction
    // we need to inverse the direction because the player is facing the opposite way
    if (interactable.Side != null && interactable.Side != player.FacingDirection.Inverse()) { return; }

    interactable.OnInteraction.Emit();

  }

}