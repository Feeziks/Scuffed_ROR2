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

  private ObjectPool<GameObject> itemPool;
  private List<GameObject> instantiatedItems;

  private Dictionary<Constants.ItemRarity, List<SO_Item>> itemsByRarity;
  private List<SO_Item> allEquipments;

  //TODO Remove this, currently is just for testing
  private float spawnTimer;
  private float spawnInterval = 2f;

  private void Awake()
  {
    itemPool = new ObjectPool<GameObject>();
    instantiatedItems = new List<GameObject>();
    InitializeItemPool();
    itemsByRarity = new Dictionary<Constants.ItemRarity, List<SO_Item>>();
    allEquipments = new List<SO_Item>();
    LoadAllItems();

    spawnTimer = Time.realtimeSinceStartup;
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    if (Time.realtimeSinceStartup - spawnTimer > spawnInterval)
    {
      spawnTimer = Time.realtimeSinceStartup;
      var values = System.Enum.GetValues(typeof(Constants.ItemRarity));
      Constants.ItemRarity randomRarity = (Constants.ItemRarity)values.GetValue((int)Random.Range(0, values.Length - 1));
      SpawnRandomItemByRarity(randomRarity);
    }
  }

  public void SpawnRandomItem()
  {

  }

  public void SpawnRandomItemByRarity(Constants.ItemRarity rarity)
  {
    SO_Item randomItem = itemsByRarity[rarity][(int)Random.Range(0f, itemsByRarity[rarity].Count)];
    GameObject thisGo = itemPool.Get();
    thisGo.SetActive(true);
    thisGo.transform.localScale = new Vector3(10, 10, 10);
    Item thisItem = thisGo.GetComponent(typeof(Item)) as Item;
    thisItem.SetSO_Item(randomItem);
    thisItem.SpawnItem();
    instantiatedItems.Add(thisGo);
    PlaceItemRandomly(thisGo);
  }

  public void SpawnItem()
  {
    //For now we just want to spawn the test items to show that they work
    instantiatedItems.Add(itemPool.Get());
  }

  public void PlaceItemRandomly(GameObject item)
  {
    item.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(10, 30f), Random.Range(-10f, 10f));
  }

  private void OnItemPickup(GameObject go)
  {
    if(!instantiatedItems.Contains(go))
    {
      Debug.LogError("Something went wrong an item not pulled from the item pool was instantiated and picked up by the player");
      return;
    }

    //Reset the item and return it to the object pool
    Item i = (Item)go.GetComponent(typeof(Item));
    i.ResetItem();
    itemPool.Return(go);
  }

  public List<SO_Item> GetAllItemsOfRarity(Constants.ItemRarity rarity)
  {
    return itemsByRarity[rarity];
  }

  public SO_Item GetItemByID(Constants.ItemID id)
  {
    //TODO: There is probably a better way to do this
    foreach(SO_Item i in itemsByRarity[id.GetRarity()])
    {
      if (i.ID == id)
        return i;
    }

    return null;
  }

  private void InitializeItemPool()
  {
    int count = 0;
    foreach (GameObject go in itemPool.pool)
    {
      go.transform.parent = transform;
      go.name = "ItemPoolObject_" + count.ToString();
      count++;

      SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
      sc.center = Vector3.zero;
      sc.radius = 2f;
      sc.enabled = false; //Only enable to collider when the item is spawned in
      sc.isTrigger = true;

      ParticleSystem ps = go.AddComponent(typeof(ParticleSystem)) as ParticleSystem;
      ps.Stop();
      MeshRenderer mr = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
      mr.enabled = false;
      MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;

      Item i = go.AddComponent(typeof(Item)) as Item;

      i.so_item = null; //Only set the SO item when the item is spawned in
      i.meshRenderer = mr;
      i.meshFilter = mf;
      i.sc = sc;
      i.ps = ps;

      go.SetActive(false);
    }
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
