using System.Collections;
using System.Collections.Generic;

public class TreeNode<T> where T : class
{
  private T _data;
  private List<TreeNode<T>> children;

  public TreeNode(T data)
  {
    _data = data;
    children = new List<TreeNode<T>>();
  }

  public void AddChild(T data)
  {
    children.Add(new TreeNode<T>(data));
  }

  public TreeNode<T> GetChildByIndex(int idx)
  {
    if (children.Count <= idx)
      return null;

    return children[idx];
  }

  public TreeNode<T> GetChildByValue(T value)
  {
    foreach(TreeNode<T> n in children)
    {
      if (n.GetData() == value)
        return n;
    }

    return null;
  }

  public List<TreeNode<T>> GetChildren()
  {
    return children;
  }

  public T GetData()
  {
    return _data;
  }
}
