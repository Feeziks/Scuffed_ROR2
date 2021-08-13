using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityTree
{
  //An ability Tree stores a tree of abilities from the root (starts unlocked for player to use)
  //To the leaf's (last ability in a line)

  //Public members
  public string abilityTreeName;

  //Private members
  private TreeNode<Ability> root;

  public AbilityTree(Ability a, string n)
  {
    root = new TreeNode<Ability>(a);
    abilityTreeName = n;
  }

  public AbilityTree(Ability a)
  {
    root = new TreeNode<Ability>(a);
    abilityTreeName = "";
  }

  public AbilityTree()
  {
    //The root will be null when constructor with no arguments is used
    root = new TreeNode<Ability>(null);
    abilityTreeName = "";
  }



  //Helper Methods
  public void Print()
  {
    Debug.Log("Printing AbilityTree: " + abilityTreeName);
    PrintRecurse(root);
  }

  private void PrintRecurse(TreeNode<Ability> n)
  {
    n.GetData().Print();
    foreach(TreeNode<Ability> c in n.GetChildren())
    {
      PrintRecurse(c);
    }
  }
}
