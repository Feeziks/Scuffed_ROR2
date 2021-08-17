using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  //For now this class wil spawn items around the map randomly using 2D perlin noise

  //Im the future this will instead spawn chests of varying types around the map (again 2D perlin noise)
  //Each chest will have its list of possible item types and some cost associated with it
  //The item within the chest should probably be determined when the chest is created - less overhead at time of opening chest? idk if that reall is a performance change at all
  //But that is likely important so that seeds can remain consistent across every game

  //TODO: Script needs some kind of collections of items
  //Either one large collection of every item and we can control which rarity / type we pick from there 
  //Or seperate collections for each one of those - either way we need the data

  public ObjectPool<Item> itemPool;

  private void Awake()
  {
    itemPool = new ObjectPool<Item>();
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    
  }


  public void SpawnItems()
  {
    //For now we just want to spawn the test items to show that they work

  }

}
