using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability Tree")]
public class SO_AbilityTree : ScriptableObject
{
  [System.Serializable]
  public class Node2
  {
    public SO_Ability ability;
    public List<Node2> children;

    public Node2()
    {
      ability = null;
      children = new List<Node2>();
    }
  }

  public string abilityTreeName;
  public Node2 root;
}
