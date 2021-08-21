using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  public SO_Item so_item;

  public ParticleSystem ps;
  public SphereCollider sc;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;

  public EventManager eManager;

  private bool spawned;
  private float rotationHz = 1f;

  public void SetSO_Item(SO_Item s)
  {
    so_item = s;

    if(so_item.model != null)
    {
      meshFilter.mesh = so_item.model;
      meshRenderer.material = so_item.material;
    }
  }

  public bool SpawnItem()
  {
    if (so_item == null)
      return false;

    sc.enabled = true;
    meshRenderer.enabled = true;
    ps.Stop();

    spawned = true;

    return true;
  }

  public void ResetItem()
  {
    so_item = null;
    ps.Stop();
    sc.enabled = false;
    meshRenderer.enabled = false;
    meshFilter.mesh = null;
    spawned = false;
  }


  private void Awake()
  {
    spawned = false;
  }
  private void Start()
  {
  }

  private void Update()
  {
    if(spawned)
    {
      transform.Rotate(transform.up, 360f / (rotationHz / Time.deltaTime));
      transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.realtimeSinceStartup) * 3f + 30f, transform.position.z);
    }
  }

  
  private void OnTriggerEnter(Collider other)
  {
    //If the other collider is a player give them the item
    if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      //TODO How to get player number from the collision?
      OnItemPickupDataClass temp = new OnItemPickupDataClass(so_item, gameObject, 0);
      eManager.SendMessage("OnItemPickup", temp);
    }
  }
  
}
