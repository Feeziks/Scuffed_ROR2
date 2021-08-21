using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Active - Equipment")]
public class SO_Item_Equipment : SO_Item
{
  [Header("Equipment Specific Stats")]
  public float procCoeff;

  //TODO: Store the action that the equipment will fire when its activated
  public SO_ScriptableAction onCastAction;
}
