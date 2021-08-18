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

  public ObjectPool<Item> itemPool;

  private Dictionary<Constants.ItemRarity, List<SO_Item>> itemsByRarity;
  private List<SO_Item> allEquipments;

  private void Awake()
  {
    itemPool = new ObjectPool<Item>();
    itemsByRarity = new Dictionary<Constants.ItemRarity, List<SO_Item>>();
    allEquipments = new List<SO_Item>();
    LoadAllItems();
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

  private void LoadAllItems()
  {
    Object[] allItems = Resources.LoadAll("Items", typeof(SO_Item));

    foreach(Constants.ItemRarity key in System.Enum.GetValues(typeof(Constants.ItemRarity)))
    {
      itemsByRarity[key] = new List<SO_Item>();
    }
    
    //Put the items into the correct dictionary
    foreach(Object item in allItems)
    {
      SO_Item itemCasted = (SO_Item)item;
      if(itemCasted.ID.IsEquipment())
      {
        allEquipments.Add(itemCasted);
      }
      else
      {
        itemsByRarity[itemCasted.ID.GetRarity()].Add(itemCasted);
      }
    }
  }

}
