using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class SO_Item : ScriptableObject
{
  public string itemName;
  public Constants.ItemID ID;
  public string shortDescription;
  public string longDescription;

  public GameObject model;
  public Sprite sprite;

  //TODO: How to store something that says what this item will modify? I.E. this item affects move speed / attack speed etc.

  public float firstStackModifier;
  public float additionalStackModifier;
}
