using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class, new()
{
  //The "pool"
  private Stack<T> pool = new Stack<T>(initialDepth);
  private static int initialDepth = 1024;
  private int currentDepth;

  public ObjectPool()
  {
    currentDepth = initialDepth;

    //Initialize the items in the pool
    for(int i = 0; i < initialDepth; i++)
    {
      T temp = new T();
      pool.Push(temp);
    }
  }

  public T Get()
  { 
    if(pool.Count == 0)
    {
      ResizePool();
    }

    return pool.Pop();
  }

  public List<T> Get(int num)
  {
    if(num <= 0)
    {
      throw new System.ArgumentOutOfRangeException("Cannot request a negative number from the object pool");
    }

    List<T> toRet = new List<T>();
    for(int i = 0; i < num; i++)
    {
      toRet.Add(Get());
    }

    return toRet;
  }

  public void Return(T obj)
  {
    pool.Push(obj);
  }

  public void Return(List<T> obj)
  {
    foreach(T o in obj)
    {
      pool.Push(o);
    }
  }

  private void ResizePool()
  {
    //My thinking is that this is only called when the stack is empty so no need for us to copy data over to it
    //tbd if that is accurate
    currentDepth *= 2;
    pool = new Stack<T>(currentDepth);
    //haf size because we are going to expect the other half to be returned eventually
    for(int i = 0; i < currentDepth / 2; i++)
    {
      T temp = new T();
      pool.Push(temp);
    }
  }
}
