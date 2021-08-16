using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  public SO_Item so_item;

  private float rotationHz = 0.5f; //2 seconds to rotate 360 degrees

  public ParticleSystem particleSystem;
  public Collider collider;
  private GameObject model;

  private void Awake()
  {
    
  }

  private void Start()
  {
    model = Instantiate(so_item.model);
    model.transform.SetParent(transform);

    transform.position = new Vector3(0f, 80f, 0f);
    transform.localScale = new Vector3(10f, 10f, 10f);
  }

  private void Update()
  {
    //Rotate the item slowly
    model.transform.RotateAround(transform.position, transform.up, Time.deltaTime * (360.0f / rotationHz));
  }

  public void SpawnItems()
  {
    //TODO : Spawn items randomly with perlin noise
  }

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
}
