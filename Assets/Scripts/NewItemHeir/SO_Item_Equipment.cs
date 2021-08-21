using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item - Equipment Active")]
public class SO_Item_Equipment : SO_Item2
{
  [Header("Equipment Specific Stats")]
  public float procCoeff;

  //TODO: Store the action that the equipment will fire when its activated
}
