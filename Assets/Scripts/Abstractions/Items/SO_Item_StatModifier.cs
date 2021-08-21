using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Passive - Stat Modifier")]
public class SO_Item_StatModifier : SO_Item
{
  [Header("Item Specific Stats")]
  public Constants.StatItemModifies statModified;
  public float firstStackModifier;
  public float additionalStackModifier;
}
