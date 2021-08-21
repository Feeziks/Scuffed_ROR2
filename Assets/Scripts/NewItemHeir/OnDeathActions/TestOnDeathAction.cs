using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemEffects/OnDeathEffects/TestEffect")]
public class TestOnDeathAction : SO_ScriptableAction
{
  public override void PerformAction(GameObject go)
  {
    //Just smack a giant cube on top of the thing
    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
    temp.transform.localPosition = go.transform.localPosition;
    temp.transform.localScale = new Vector3(300f, 300f, 300f);
  }
}
