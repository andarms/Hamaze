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

  private void ProcessInteraction()
  {
    var hit = CollisionsManager.GetPotentialCollisions(this).FirstOrDefault(x => x.Traits.Has<Interactable>());
    if (hit == null) return;

    Interactable interactable = hit.Traits.Get<Interactable>();
    if (interactable == null) return;

    if (interactable.Side != player.FacingDirection.Inverse()) { return; }

    interactable.OnInteraction.Emit();

  }

  private void UpdatePosition()
  {
    var playerCenter = player.Collider.Size * 0.5f;
    var offset = player.FacingDirection.ToVector2() * player.Collider.Size + player.Collider.Offset;
    Position = playerCenter + offset - size * 0.5f;
  }
}