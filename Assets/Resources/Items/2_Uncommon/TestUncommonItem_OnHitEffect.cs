using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemEffects/OnHitEffects/TestEffect")]
public class TestUncommonItem_OnHitEffect : SO_ScriptableAction
{
  public override void PerformAction(GameObject go)
  {
    //Make a sphere or something I dont know squish it for neatness
    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    sphere.transform.position = go.transform.position;
    sphere.transform.localScale = new Vector3(10f, 1f, 1f);
    Destroy(sphere, 4f);
  }
}
