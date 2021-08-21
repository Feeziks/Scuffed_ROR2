using System;
using UnityEngine;

public class SO_Item2 : ScriptableObject, IComparable
{
  //Things that every item regardless of type will be required to have
  [Header("Item Base Stats")]
  public Constants.ItemID id;

  public string itemName;
  public string tooltipDescription;
  public string longDescription;

  public Sprite sprite;
  public Mesh model;
  public Material material;


  public int CompareTo(object obj)
  {
    if(obj == null)
    {
      return 1;
    }

    SO_Item2 otherItem = obj as SO_Item2;
    if (otherItem != null)
    {
      return this.id.CompareTo(otherItem.id);
    }
    else
      throw new ArgumentException("Object is no an SO_Item2");
  }
}
