using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemies/Enemy Settings")]
public class SO_EnemySettings : ScriptableObject
{
  public bool flying; //True = flying; False = walking - TODO: make it so enemies can fly or walk?

  public float moveSpeed;
  public float jumpHeight;

  public float maxHealth;
  public float healthRegen;

  public float damage;

  public float moneyReward;
  public float expReward;
}
