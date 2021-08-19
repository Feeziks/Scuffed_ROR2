using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  public SO_Item so_item;

  public ParticleSystem particleSystem;
  public SphereCollider collider;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;

  private bool spawned;
  private float rotationHz = 0.5f;

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

    collider.enabled = true;
    meshRenderer.enabled = true;

    return true;
  }

  public void ResetItem()
  {
    so_item = null;
    particleSystem.Stop();
    collider.enabled = false;
    meshRenderer.enabled = false;
    meshFilter.mesh = null;
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
      transform.Rotate(transform.up, rotationHz / Time.deltaTime);
    }
  }

  
  private void OnTriggerEnter(Collider other)
  {
    //If the other collider is a player give them the item
    if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      other.gameObject.SendMessage("OnItemPickup", so_item);
      transform.parent.SendMessage("OnItemPickup", gameObject);
      //TODO: Add some particle effect stuff here
      //TODO: Add some sound stuff here
    }
  }
  
}
