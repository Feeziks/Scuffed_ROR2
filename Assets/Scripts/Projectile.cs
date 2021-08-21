using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public SO_Projectile projectileData;
  public MeshFilter mf;
  public MeshRenderer mr;
  public SphereCollider sc;

  public bool spawned = false;
  private float lifeTime;

  private void Start()
  {
    mf.mesh = projectileData.model;
    transform.localScale = new Vector3(2f, 2f, 1f);
  }

  private void Update()
  {
    if(spawned)
    {
      //move forwards in our direction at our velocity
      transform.position += transform.forward * projectileData.velocity * Time.deltaTime;

      if(projectileData.useGravity)
      {
        float newY = transform.position.y + projectileData.gravity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
      }

      if(Time.realtimeSinceStartup - lifeTime > projectileData.lifeSpan)
      {
        mr.enabled = false;
        gameObject.SetActive(false);
        spawned = false;
        //TODO: Return this item to the players projectile pool
      }
    }
  }

  public void Spawn(Vector3 pos, Vector3 direction)
  {
    transform.position = pos;
    transform.rotation.SetLookRotation(direction);
    sc.enabled = true;
    mr.enabled = true;
    spawned = true;
    gameObject.SetActive(true);
    lifeTime = Time.realtimeSinceStartup;
  }

  private void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    {
      //TODO Create accurate damage  type information 
      DamageType data = new DamageType(20, 1, 0);
      other.gameObject.SendMessage("RecieveDamage", data);
      gameObject.SetActive(false);
      spawned = false;
    }
    
    if(other.gameObject.layer == LayerMask.NameToLayer("GroundLayer"))
    {
      gameObject.SetActive(false);
      spawned = false;
    }
  }
}
