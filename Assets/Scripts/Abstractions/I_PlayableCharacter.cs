using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Interface All playable characters will be required to implement
 * Every function and property in the interface will be necessary for each character
 */
public interface I_PlayableCharacter
{
  //Members
  PlayableCharacterSettings characterSettings { get; set; }

  //Methods
  void OnPrimaryAbilityAction(InputAction.CallbackContext context);

  void OnSecondaryAbilityAction(InputAction.CallbackContext context);

  void RecieveDamage(float damage, Constants.ElementTypes elemType);
  void InitSettings();
}
