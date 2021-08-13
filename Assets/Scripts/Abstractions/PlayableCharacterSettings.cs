using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableCharacterSettings
{
  /*
   ***** Public settings
  */

  //Movement
  public float baseMoveSpeed;
  public float moveSpeedModifier;

  public float baseSprintSpeedMultiplier;
  public float sprintSpeedModifier;

  public float baseJumpHeight;
  public float jumpHeightModifier;

  public uint  baseNumberJumps;
  public int   numberJumpsModifier;
  
  //Defense
  public float baseHealth;
  public float healthModifier;

  public float baseArmor;
  public float armorModifier;

  public Dictionary<Constants.ElementTypes, float> baseElementalResistances;
  public Dictionary<Constants.ElementTypes, float> elementalResistanceModifier;


  /*
   ***** Public Members
   */
  //Constructor
  public PlayableCharacterSettings()
  {
    baseElementalResistances = new Dictionary<Constants.ElementTypes, float>();
    elementalResistanceModifier = new Dictionary<Constants.ElementTypes, float>();
  }

  //Helper function
  public void Print()
  {
    Debug.Log("baseMoveSpeed: " + baseMoveSpeed);
    Debug.Log("moveSpeedModifier: " + moveSpeedModifier);

    Debug.Log("baseJumpHeight: " + baseJumpHeight);
    Debug.Log("jumpHeightModifier: " + jumpHeightModifier);

    Debug.Log("baseNumberJumps: " + baseNumberJumps);
    Debug.Log("numberJumpsModifier: " + numberJumpsModifier);

    Debug.Log("baseHealth: " + baseHealth);
    Debug.Log("healthModifier: " + healthModifier);

    Debug.Log("baseArmor: " + baseArmor);
    Debug.Log("armorModifier: " + armorModifier);

    foreach (KeyValuePair<Constants.ElementTypes, float> kvp in baseElementalResistances)
    {
      Debug.Log("baseElementalResistance: " + kvp.Key + " " + kvp.Value);
      Debug.Log("elementalResistanceModifier: " + kvp.Key + " " + elementalResistanceModifier[kvp.Key]);
    }
  }

}
