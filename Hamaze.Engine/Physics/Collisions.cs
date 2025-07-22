using System.Numerics;

namespace Hamaze.Engine.Physics;

public record Collision(PhysicsObject ObjectA, PhysicsObject ObjectB, Vector2 Normal, float Penetration) { }


