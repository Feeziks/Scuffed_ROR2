using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Settings")]
public class SO_PlayableCharacterSettings : ScriptableObject
{
  //Movement
  public float baseMoveSpeed;
  public float baseSprintSpeedMultiplier;
  public float baseJumpHeight;
  public int   baseNumberJumps;
  //Defense
  public float baseHealth;
  public float baseArmor;
  public Dictionary<Constants.ElementTypes, float> baseElementalResistances;
}
