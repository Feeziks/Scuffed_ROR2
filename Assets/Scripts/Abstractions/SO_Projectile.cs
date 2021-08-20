using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Projectile")]
public class SO_Projectile : ScriptableObject
{
  public Mesh model;

  public float velocity;
  public bool useGravity;
  public float gravity;

  public float colliderRadius;

  public float lifeSpan;
}
