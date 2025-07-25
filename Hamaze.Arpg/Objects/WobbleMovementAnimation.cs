using Hamaze.Engine.Core;
using Hamaze.Engine.Graphics;
using Hamaze.Engine.Physics;
using Microsoft.Xna.Framework;

namespace Hamaze.Arpg.Objects;

public class WobbleMovementAnimation(Sprite sprite, DynamicObject character) : GameObject
{
    public float AnimationSpeed { get; set; } = 2f;
    private readonly float maxRotation = 0.1f;

    public override void Update(float dt)
    {
        base.Update(dt);
        if (character.Velocity == Vector2.Zero)
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