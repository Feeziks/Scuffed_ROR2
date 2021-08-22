using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
  public SO_EnemyData enemyData;

  public NavMeshAgent navMeshAgent;
  public float currHealth;

  public List<PlayerController> players;

  public Transform target;
  public MeshFilter mf;
  public Rigidbody rb;
  public MeshRenderer mr;
  public EventManager eManager;

  private float reTargetTimer = 0.5f;
  private float lastRetargetTime;

  private bool spawned;

  private void Start()
  {
    lastRetargetTime = Time.realtimeSinceStartup;
    spawned = false;
  }

  private void Update()
  {
    if (spawned)
    {
      if (Time.realtimeSinceStartup - lastRetargetTime > reTargetTimer)
      {
        lastRetargetTime = Time.realtimeSinceStartup;
        //Find which player is closest
        float closestDist = float.MaxValue;
        foreach (PlayerController player in players)
        {
          float distance = Vector3.Distance(transform.position, player.transform.position);
          if (distance < closestDist)
          {
            target = player.transform;
          }
        }
      }
      navMeshAgent.SetDestination(target.position);
    }
  }


  public void Spawn(SO_EnemyData enemy, Vector3 pos)
  {
    enemyData = enemy;
    transform.position = pos;

    if(enemy.spawnAction != null)
    {
      enemy.spawnAction.PerformAction(gameObject);
    }

    navMeshAgent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;

    gameObject.SetActive(true);

    navMeshAgent.speed = enemyData.settings.moveSpeed;
    currHealth = enemyData.settings.maxHealth;
  }

  public void Reset()
  {
    enemyData = null;
    currHealth = -1;
    target = null;
    mr.enabled = false;
    spawned = false;
  }
  private void RecieveDamage(DamageType damage)
  {
    currHealth -= damage.value;
    if(currHealth <= 0)
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
    if(enemyData.deathAction != null)
    {
      enemyData.deathAction.PerformAction(gameObject);
    }

    EnemyDeathDataType data = new EnemyDeathDataType(gameObject, enemyData, damage.value, damage.playerthatDealtDamage);
    eManager.SendMessage("OnEnemyDeath", data);
  }
}
