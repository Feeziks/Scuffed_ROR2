using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Event Data Types
public class EnemyDeathDataType
{
  public GameObject enemy;
  public float finalBlowDamage;
  public int playerThatKilledEnemy;

  public EnemyDeathDataType(GameObject e, float d, int p)
  {
    enemy = e;
    finalBlowDamage = d;
    playerThatKilledEnemy = p;
  }
}

public class OnEnemyHitDataType
{
  public GameObject enemy;
  public float damageDealt;
  public int playerThatHitEnemy;

  public OnEnemyHitDataType(GameObject e, float d, int p)
  {
    enemy = e;
    damageDealt = d;
    playerThatHitEnemy = p;
  }
}

public class OnItemPickupDataClass
{
  public SO_Item item;
  public GameObject itemGO;
  public int player;

  public OnItemPickupDataClass(SO_Item i, GameObject go, int p)
  {
    item = i;
    itemGO = go;
    player = p;
  }
}

#endregion

#region Damage Data Types

public class DamageType
{
  //TODO: Want to be able to apply / have elemental damage and normal damage probably just a Dictionary 

  public float value;
  public float runningProcCoeff;
  public int playerthatDealtDamage;

  public DamageType(float d, float pc, int player)
  {
    value = d;
    runningProcCoeff = pc;
    playerthatDealtDamage = player;
  }
}

#endregion