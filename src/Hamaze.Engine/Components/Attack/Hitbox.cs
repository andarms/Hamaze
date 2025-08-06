using System;
using System.Xml.Linq;
using Hamaze.Engine.Collisions;
using Hamaze.Engine.Core;
using Hamaze.Engine.Data;


namespace Hamaze.Engine.Components.Attack;

public class Hitbox : GameObject
{
  [Save]
  public IDamageCalculator DamageCalculator { get; set; } = new NoDamage();

  public Hitbox()
  {
    CollisionsManager.AddObject(this);
  }

  // public override void Deserialize(XElement element)
  // {
  //   base.Deserialize(element);
  //   var damageElement = element.Element(DamageSerializer.ElementName);
  //   if (damageElement == null)
  //   {
  //     return;
  //   }
  //   Damage = DamageSerializer.Deserialize(damageElement);
  // }
}