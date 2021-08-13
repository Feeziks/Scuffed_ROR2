using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Data")]
public class SO_CharacterData : ScriptableObject
{
  //Character Name
  public string characterName;

  //Character Model

  //Character Animations

  //Character Settings
  public PlayableCharacterSettings characterSettings;

  //Character Abilities
  public List<AbilityTree> characterAbilities;
}
