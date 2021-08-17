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




  public ModifiedSettings()
  {
    elementalResistances = new Dictionary<Constants.ElementTypes, float>();
    foreach(Constants.ElementTypes element in System.Enum.GetValues(typeof(Constants.ElementTypes)))
    {
      elementalResistances[element] = 0.0f;
    }
  }
}
