using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

  public List<PlayerController> players;
  private ObjectPool<GameObject> enemiesPool;

  private List<SO_EnemyData> allEnemies;
  //TODO: Fix this as its just for testing now
  private float spawnTimer;
  private float spawnInterval = 3f;

  #region Unity Methods
  private void OnEnable()
  {
    EventManager.enemyDeathEvent += OnEnemyDeath;
  }

  private void OnDisable()
  {
    EventManager.enemyDeathEvent -= OnEnemyDeath;
  }

  private void Awake()
  {
    enemiesPool = new ObjectPool<GameObject>(256); 
  }

  private void Start()
  {
    InitializeEnemyPool();
    LoadAllEnemies();

    spawnTimer = Time.realtimeSinceStartup;
  }

  private void Update()
  {
    if(Time.realtimeSinceStartup - spawnInterval > spawnTimer)
    {
      SpawnEnemy(allEnemies[(int)Random.Range(0f, allEnemies.Count)], new Vector3(Random.Range(-20f, 21f), 20f, Random.Range(-50f, 50f)));
    }
  }

  private void FixedUpdate()
  {
    
  }

  #endregion


  #region Initializers & Helpers
  private void LoadAllEnemies()
  {
    allEnemies = new List<SO_EnemyData>();
    Object[] loadedEnemies = Resources.LoadAll("Enemies", typeof(SO_EnemyData));

    foreach(Object enemy in loadedEnemies)
    {
      allEnemies.Add((SO_EnemyData)enemy);
    }
  }

  #endregion

  #region object pooling
  private void InitializeEnemyPool()
  {
    int count = 0;
    EventManager eManager = FindObjectOfType(typeof(EventManager)) as EventManager;
    foreach(GameObject go in enemiesPool.pool)
    {
      go.transform.parent = transform;
      go.name = "EnemyPoolObject_" + count.ToString();
      go.layer = LayerMask.NameToLayer("Enemy");
      go.transform.position = new Vector3(0f, 30f, 0f);
      count++;

      go.SetActive(false);

      Rigidbody rb = go.AddComponent(typeof(Rigidbody)) as Rigidbody;
      MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
      MeshRenderer mr = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
      mr.enabled = false;

      EnemyAgent ea = go.AddComponent(typeof(EnemyAgent)) as EnemyAgent;
      ea.eManager = eManager;
      ea.enemyData = null;
      ea.players = players;//TODO: Find better way to send enemy manager players
    }
    Debug.Log("Sucking thing fucks");
  }

  public void ReturnEnemyToPool(GameObject go)
  {
    go.SetActive(false);
    EnemyAgent ea = go.GetComponent(typeof(EnemyAgent)) as EnemyAgent;
    ea.Reset();
    enemiesPool.Return(go);
  }

  public void SpawnEnemy(SO_EnemyData enemyToSpawn, Vector3 spawnPosition)
  {
    GameObject go = enemiesPool.Get();
    go.SetActive(true);
    EnemyAgent ea = go.GetComponent(typeof(EnemyAgent)) as EnemyAgent;
    ea.Spawn(enemyToSpawn, spawnPosition);
  }

  #endregion

  #region Event Handling
  public void OnEnemyDeath(EnemyDeathDataType data)
  {
    ReturnEnemyToPool(data.enemyGO);
  }

  #endregion
}
