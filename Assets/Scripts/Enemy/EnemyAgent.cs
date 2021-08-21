using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
  public SO_EnemyData enemyData;

  public NavMeshAgent navMeshAgent;
  public float speed;
  public float maxHealth;
  public float currHealth;

  public PlayerController player;

  public Transform target;

  public EventManager eManager;

  private void Start()
  {
    navMeshAgent.speed = speed;
    currHealth = maxHealth;
  }

  private void Update()
  {
    navMeshAgent.SetDestination(target.position);
  }

  private void RecieveDamage(DamageType damage)
  {
    currHealth -= damage.value;
    if(currHealth < 0)
    {
      Die(damage);
    }
    else
    {
      OnEnemyHitDataType data = new OnEnemyHitDataType(gameObject, damage.value, damage.playerthatDealtDamage);
      eManager.SendMessage("OnEnemyHit", data);
    }
  }

  private void Die(DamageType damage)
  {
    EnemyDeathDataType data = new EnemyDeathDataType(gameObject, damage.value, damage.playerthatDealtDamage);
    eManager.SendMessage("OnEnemyDeath", data);
  }
}
