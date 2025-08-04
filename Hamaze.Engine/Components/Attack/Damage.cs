using System;
using System.Xml.Linq;

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



public static class DamageSerializer
{
  public const string ElementName = "DamageCalculator";
  public static XElement Serialize(IDamageCalculator damageCalculator)
  {
    var element = new XElement(ElementName);
    if (damageCalculator is NoDamage)
    {
      element.SetAttributeValue("Type", "NoDamage");
      return element;
    }

    if (damageCalculator is SimpleDamage simpleDamage)
    {
      element.SetAttributeValue("Type", "SimpleDamage");
      element.Add(new XElement("Amount", simpleDamage.Amount));
      return element;
    }

    throw new NotSupportedException("Unknown damage calculator type.");
  }

  public static IDamageCalculator Deserialize(XElement element)
  {
    var type = element.Attribute("Type")?.Value;
    if (type == "NoDamage") return new NoDamage();

    if (type == "SimpleDamage")
    {
      var amount = int.Parse(element.Element("Amount")?.Value ?? "0");
      return new SimpleDamage(amount);
    }

    throw new NotSupportedException("Unknown serialized damage calculator.");
  }
}