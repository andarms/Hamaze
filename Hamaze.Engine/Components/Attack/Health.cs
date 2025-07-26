using System;
using Hamaze.Engine.Events;

namespace Hamaze.Engine.Components.Attack;

public class Health(int current, int max)
{
  public int Current { get; private set; } = current;
  public int Max { get; private set; } = max;
  public bool IsDead => Current <= 0;
  public Signal Dead { get; } = new();

  public void TakeDamage(int amount)
  {
    if (IsDead) return;

    Current -= amount;
    if (Current < 0)
    {
      Current = 0;
      Dead.Emit();
    }
    Console.WriteLine($"Health: {Current}/{Max}");
  }

  public void Heal(int amount)
  {
    if (IsDead) return;

    Current += amount;
    if (Current > Max) { Current = Max; }
  }
}