using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Data")]
public class SO_EnemyData : ScriptableObject
{
  public Mesh model;

  public SO_EnemySettings settings;

  public List<SO_Ability> abilities;
}
