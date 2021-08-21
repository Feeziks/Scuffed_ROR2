﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
  public GameObject itemInventoryPanel;
  public TextMeshProUGUI moneyText;
  public int player;

  public float xSpacing = 10f;
  public float xLimit = 800f;
  public float ySpacing = 10f;

  private SortedDictionary<Constants.ItemID, GameObject> itemInventoryToSpriteGameObject;
  private SortedDictionary<Constants.ItemID, TextMeshProUGUI> itemInventoryToText;
  private Dictionary<Constants.ItemID, RectTransform> itemInventoryToRectTransform;

  private ItemManager itemManager;

  private void OnEnable()
  {
    EventManager.itemPickupEvent += ItemPickup;
  }

  private void OnDisable()
  {
    EventManager.itemPickupEvent -= ItemPickup;
  }

  private void Awake()
  {
    player = 0;
    itemManager = FindObjectOfType(typeof(ItemManager)) as ItemManager;
  }

  public void Start()
  {
    itemInventoryToSpriteGameObject = new SortedDictionary<Constants.ItemID, GameObject>();
    itemInventoryToText = new SortedDictionary<Constants.ItemID, TextMeshProUGUI>();
    itemInventoryToRectTransform = new Dictionary<Constants.ItemID, RectTransform>();

    InitializeItemInventory();
  }

  #region Initializers and helpers
  private void InitializeItemInventory()
  {
    RectTransform parent = itemInventoryPanel.GetComponent(typeof(RectTransform)) as RectTransform;
    foreach (Constants.ItemRarity r in System.Enum.GetValues(typeof(Constants.ItemRarity)))
    {
      List<SO_Item> listItems = itemManager.GetAllItemsOfRarity(r);
      listItems.Sort((x, y) => x.id.CompareTo(y.id));

      foreach(SO_Item i in listItems)
      {
        GameObject newGo = new GameObject("Item_" + i.id.GetID().ToString());
        RectTransform rt = newGo.AddComponent(typeof(RectTransform)) as RectTransform;
        rt.SetParent(parent, false);
        rt.anchorMin = new Vector2(0f, 0.5f);
        rt.anchorMax = new Vector2(0f, 0.5f);
        rt.pivot = new Vector2(0, 0.5f);
        Image im = newGo.AddComponent(typeof(Image)) as Image;
        im.sprite = i.sprite;

        newGo.SetActive(false);

        itemInventoryToSpriteGameObject[i.id] = newGo;
        itemInventoryToRectTransform[i.id] = rt;

        GameObject newTextGo = new GameObject("Item_" + i.id.GetID().ToString() + "_Counter");
        RectTransform textRt = newTextGo.AddComponent(typeof(RectTransform)) as RectTransform;
        textRt.SetParent(rt, false);
        textRt.anchorMin = new Vector2(1f, 0f);
        textRt.anchorMax = new Vector2(1f, 0f);
        textRt.pivot = new Vector2(1f, 0f);
        textRt.anchoredPosition = new Vector2(0f, -10f);
        textRt.sizeDelta = new Vector2(100f, 40f);
        

        TextMeshProUGUI txt = newTextGo.AddComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        txt.enableAutoSizing = true;
        txt.alignment = TextAlignmentOptions.BottomRight;
        txt.color = Color.white;

        newTextGo.SetActive(false);

        itemInventoryToText[i.id] = txt;

      }      
    }
  }

  #endregion

  #region Item Display and Counts
  public void UpdateItemDisplay()
  {
    SortedDictionary<SO_Item, int> inventory = new SortedDictionary<SO_Item, int>();
    itemManager.GetPlayerInventory(player, ref inventory);
    foreach(KeyValuePair<SO_Item, int> kvp in inventory)
    {
      Constants.ItemID id = kvp.Key.id;
      if(kvp.Value == 1)
      {
        //First item of this kind picked up -> add its sprite into the item inventory in order of item ID
        itemInventoryToSpriteGameObject[id].SetActive(true);
        //Move everything around to be in the right order
      }
      else if(kvp.Value > 1)
      {
        //Multiple of this item held add text to its sprite to display the number of stacks it has
        itemInventoryToText[id].gameObject.SetActive(true);
        itemInventoryToText[id].text = "x" + kvp.Value;

      }
    }
    OrderItemSprites();
  }

  private void OrderItemSprites()
  {
    // Look at the items that are "active" / "held" by the player and organize the sprites by the item ID order
    //Check which GameObjects are active - if they are active then we have at least one of those items

    float xPos = xSpacing;
    float yPos = ySpacing;

    foreach(KeyValuePair<Constants.ItemID, GameObject> kvp in itemInventoryToSpriteGameObject)
    {
      if (kvp.Value.activeSelf)
      {
        itemInventoryToRectTransform[kvp.Key].anchoredPosition = new Vector2(xPos, yPos);
        xPos += itemInventoryToRectTransform[kvp.Key].rect.width + xSpacing;

        if(xPos >= xLimit)
        {
          xPos = xSpacing;
          yPos += ySpacing;
        }
      }
    }

  }

  #endregion

  #region Money Display

  public void UpdateMoney(float money)
  {
    moneyText.text = money.ToString();
  }

  #endregion

  #region Event Handling

  public void ItemPickup(OnItemPickupDataClass data)
  {
    UpdateItemDisplay();
  }

  #endregion

}
