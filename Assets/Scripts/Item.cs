using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
  public SO_Item so_item;

  public ParticleSystem particleSystem;
  public SphereCollider collider;

  public Item()
  {
    //Create the particle system

    //Create the gameobject


    //Create the sphere collider
    collider = new SphereCollider();
    collider.center = Vector3.zero;
    collider.radius = 2f;
    collider.enabled = true;
    collider.isTrigger = true;
  }

  public void SetSO_Item(SO_Item s)
  {
    so_item = s;
  }

  /*
  private void OnTriggerEnter(Collider other)
  {
    //If the other collider is a player give them the item
    if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      other.gameObject.SendMessage("OnItemPickup", so_item.ID);
      //TODO: Add some particle effect stuff here
      //TODO: Add some sound stuff here
      Destroy(this.gameObject, 0.25f);
    }
  }
  */
}
