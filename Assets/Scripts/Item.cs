using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
  public SO_Item so_item;

  public ParticleSystem particleSystem;
  public Collider collider;

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
