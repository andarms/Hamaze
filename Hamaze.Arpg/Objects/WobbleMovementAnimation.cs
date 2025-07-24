using System;
using Hamaze.Arpg.Objects.Player;
using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class WobbleMovementAnimation(Sprite sprite, Player.Player player) : GameObject
{
    public float AnimationSpeed { get; set; } = 2f;
    private readonly float maxRotation = 0.1f;

    public override void Update(float dt)
    {
        base.Update(dt);
        if (player.Velocity == Vector2.Zero)
        {
            sprite.Rotation = 0f;
            return;
        }
        sprite.Rotation += dt * AnimationSpeed;
        if (sprite.Rotation > maxRotation)
        {
            sprite.Rotation = maxRotation;
            AnimationSpeed *= -1;
        }
        else if (sprite.Rotation < -maxRotation)
        {
            sprite.Rotation = -maxRotation;
            AnimationSpeed *= -1;
        }
    }
}