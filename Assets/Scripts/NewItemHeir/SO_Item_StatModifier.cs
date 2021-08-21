using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item - Stat Modifier Passive")]
public class SO_Item_StatModifier : SO_Item2
{
  [Header("Item Specific Stats")]
  public Constants.StatItemModifies statModified;
  public float firstStackModifier;
  public float additionalStackModifier;
}
