using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]
public class SO_Ability : ScriptableObject
{
  // Public Members
  public string abilityName;
  public string shortDescription;
  public string longDescription;

  public int level;

  public List<Sprite> sprites;

  public float cooldown; //Measured in milliseconds
  public float cost; //Not really sure if we need this

  public bool passive; //True = passive ; false = active
  public bool projectile; //True = ability creates projectiles ; false = ability does NOT create projectiles
  public SO_Projectile abilityProjectile;

  public float procCoefficent;

  //TODO: How to store *what* the ability does?
  
  //TODO: How to store a function? Cast() AttemptCast() etc

  //Helper Methods
  public void Print()
  {
    string msg = "";
    msg += abilityName;
    msg += "\n";
    msg += shortDescription;
    msg += "\n";
    msg += longDescription;
    msg += "\n";

    msg += "Level : " + level;
    msg += "\n";
    msg += "Cooldown : " + cooldown;

    msg += "Passive : " + passive;

    Debug.Log(msg);
  }
}
