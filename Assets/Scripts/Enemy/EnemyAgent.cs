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

  private void Start()
  {
    navMeshAgent.speed = speed;
    currHealth = maxHealth;
  }

  private void Update()
  {
    navMeshAgent.SetDestination(target.position);
  }

  private void RecieveDamage(float damage)
  {
    currHealth -= damage;
    if(currHealth < 0)
    {
      Die();
    }
  }

  private void Die()
  {
    Debug.Log("I dieded");
    player.gameObject.SendMessage("OnEnemyDeath", gameObject);
    gameObject.SetActive(false);
  }
}
