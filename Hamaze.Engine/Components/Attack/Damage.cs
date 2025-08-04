using System;

namespace Hamaze.Engine.Components.Attack;

public interface IDamageCalculator
{
  public int CalculateDamage();
}

public class NoDamage : IDamageCalculator
{
  public int CalculateDamage() => 0;
}

public class SimpleDamage(int amount) : IDamageCalculator
{
  public int Amount => amount;
  public float CriticalChance { get; set; } = 0.01f;
  public float CriticalMultiplier { get; set; } = 1.5f;

  private float currentCriticalChance = 0f;

  public int CalculateDamage()
  {
    currentCriticalChance += CriticalChance;
    if (Random.Shared.NextDouble() < currentCriticalChance)
    {
      currentCriticalChance = 0f;
      Console.WriteLine("Critical Hit!");
      return (int)(amount * CriticalMultiplier);
    }
    return amount;
  }
}
