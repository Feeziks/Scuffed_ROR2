using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

  public delegate void EnemyDeathAction(EnemyDeathDataType data);
  public static event EnemyDeathAction enemyDeathEvent;

  public delegate void EnemyOnHitAction(OnEnemyHitDataType data);
  public static event EnemyOnHitAction enemyHitEvent;

  public delegate void ItemPickupAction(OnItemPickupDataClass data);
  public static event ItemPickupAction itemPickupEvent;

  void OnEnemyDeath(EnemyDeathDataType data)
  {
    enemyDeathEvent(data);
  }

  void OnEnemyHit(OnEnemyHitDataType data)
  {
    enemyHitEvent(data);
  }

  void OnItemPickup(OnItemPickupDataClass data)
  {
    itemPickupEvent(data);
  }

}
