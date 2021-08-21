using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability Tree")]
public class SO_AbilityTree : ScriptableObject
{
  public class Node
  {
    public SO_Ability ability;
    public List<Node> children;

    public Node()
    {
      ability = null;
      children = new List<Node>();
    }
  }

  public string abilityTreeName;
  public Node root;
}
