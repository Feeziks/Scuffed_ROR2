using System.Collections;
using System.Collections.Generic;


public class ModifiedSettings
{
  public float moveSpeed;
  public float sprintMultiplier;
  public float jumpHeight;
  public int numJumps;

  public float maxHealth;
  public float healthRegen;

  public float armor;
  public Dictionary<Constants.ElementTypes, float> elementalResistances;

  public float attackSpeed;
  public float damage;
  public float critChance;
  public float dodgeChance;

  public ModifiedSettings()
  {
    elementalResistances = new Dictionary<Constants.ElementTypes, float>();
    foreach (Constants.ElementTypes element in System.Enum.GetValues(typeof(Constants.ElementTypes)))
    {
      elementalResistances[element] = 0.0f;
    }
  }

  public void ApplyItem(SO_Item item, int num)
  {
    //TODO: Better way to do this than large if chain - not very maintainable
    if (item.modifiesStats)
    {
      float change = item.firstStackModifier + (num - 1) * item.additionalStackModifier;
      if ((item.statModifier & Constants.StatItemModifies.moveSpeed) != Constants.StatItemModifies.none)
      {
        moveSpeed += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.sprintMultiplier) != Constants.StatItemModifies.none)
      {
        sprintMultiplier += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.jumpHeight) != Constants.StatItemModifies.none)
      {
        jumpHeight += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.numJumps) != Constants.StatItemModifies.none)
      {
        numJumps += (int)item.firstStackModifier;
      }
      if ((item.statModifier & Constants.StatItemModifies.maxHealth) != Constants.StatItemModifies.none)
      {
        maxHealth += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.healthRegen) != Constants.StatItemModifies.none)
      {
        healthRegen += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.armor) != Constants.StatItemModifies.none)
      {
        armor += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.fireResist) != Constants.StatItemModifies.none)
      {
        elementalResistances[Constants.ElementTypes.Fire] += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.waterResist) != Constants.StatItemModifies.none)
      {
        elementalResistances[Constants.ElementTypes.Water] += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.earthResist) != Constants.StatItemModifies.none)
      {
        elementalResistances[Constants.ElementTypes.Earth] += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.airResist) != Constants.StatItemModifies.none)
      {
        elementalResistances[Constants.ElementTypes.Air] += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.electricResist) != Constants.StatItemModifies.none)
      {
        elementalResistances[Constants.ElementTypes.Electric] += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.attackSpeed) != Constants.StatItemModifies.none)
      {
        attackSpeed += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.damage) != Constants.StatItemModifies.none)
      {
        damage += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.critChance) != Constants.StatItemModifies.none)
      {
        critChance += change;
      }
      if ((item.statModifier & Constants.StatItemModifies.dodgeChance) != Constants.StatItemModifies.none)
      {
        dodgeChance += change;
      }
    }
  }
}
