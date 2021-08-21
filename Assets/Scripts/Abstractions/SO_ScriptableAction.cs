using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SO_ScriptableAction : ScriptableObject
{
  public virtual void PerformAction(GameObject go)
  {
    Debug.Log("Implement the action for this object " + go.name);
  }
}
