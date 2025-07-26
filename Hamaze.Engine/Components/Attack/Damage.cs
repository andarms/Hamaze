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
  public float CriticalChance { get; set; } = 0.05f;
  public float CriticalMultiplier { get; set; } = 1.5f;

  public int CalculateDamage()
  {
    if (Random.Shared.NextDouble() < CriticalChance)
    {
      Console.WriteLine("Critical Hit!");
      return (int)(amount * CriticalMultiplier);
    }
    return amount;
  }
}