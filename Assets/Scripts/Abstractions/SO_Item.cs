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

  public Mesh model;
  public Material material;
  public Sprite sprite;

  //TRUE = does modify stats , FALSE = does something else
  public bool modifiesStats;
  public Constants.StatItemModifies statModifier;
  public float firstStackModifier;
  public float additionalStackModifier;

  //TODO: How to store what this item does other than stat modification
  //I.E. on hit enemy fire a missle etc
  public float procCoefficent;
}
