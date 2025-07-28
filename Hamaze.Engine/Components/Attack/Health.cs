using System;
using Hamaze.Engine.Core;
using Hamaze.Engine.Events;

namespace Hamaze.Engine.Components.Attack;


public class Health(int current, int max) : Trait("Health")
{
  public bool IsDead => current <= 0;
  public Signal Dead { get; } = new();
  public Signal<int> HealthChanged { get; } = new();

  public void TakeDamage(int amount)
  {
    if (IsDead) return;

    current -= amount;
    if (current < 0)
    {
      current = 0;
      Dead.Emit();
    }
    HealthChanged.Emit(current);
    Console.WriteLine($"Health: {current}/{max}");
  }

  public void Heal(int amount)
  {
    if (IsDead) return;

    current += amount;
    if (current > max) { current = max; }
    HealthChanged.Emit(current);
  }

  public override string ToString()
  {
    return $"Health: {current}/{max}";
  }
}