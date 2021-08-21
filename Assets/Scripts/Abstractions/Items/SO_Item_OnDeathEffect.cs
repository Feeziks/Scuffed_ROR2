using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Passive - On Death Effect")]
public class SO_Item_OnDeathEffect : SO_Item
{
  [Header("Item Specific Stats")]
  public float procCoeff; //Does this effect proc other effects, if so how often

  public float procChance; //Does this effect proc every time? if not how often

  //TODO: Store the function that will run when something dies - needs to be able to change for each instance of tis scriptable object we creates
  public SO_ScriptableAction onDeathAction;
}
