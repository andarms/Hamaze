using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;


namespace Hamaze.Engine.Components.Attack;

public class Hitbox : GameObject
{
  public IDamageCalculator Damage { get; set; } = new NoDamage();

  public Hitbox()
  {
    CollisionsManager.AddObject(this);
  }


  public override XElement? Serialize()
  {
    var element = base.Serialize();
    element?.Add(new XElement(DamageSerializer.Serialize(Damage)));
    return element;
  }

  public override void Deserialize(XElement element)
  {
    base.Deserialize(element);
    var damageElement = element.Element(DamageSerializer.ElementName);
    if (damageElement == null)
    {
      return;
    }
    Damage = DamageSerializer.Deserialize(damageElement);
  }
}