using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Enemy Data")]
public class SO_EnemyData : ScriptableObject
{
  public Mesh model;

  public SO_EnemySettings settings;

  public List<SO_Ability> abilities;

  public SO_ScriptableAction spawnAction;
  public SO_ScriptableAction deathAction;
}
