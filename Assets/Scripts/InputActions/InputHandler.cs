using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
  public Vector2 move;
  public Vector2 look;
  public bool jump;
  public bool sprint;
  public bool reset;

  public bool analogMovement;

  public bool cursorLocked = true;
  public bool cursorInputForLook = true;

  //Public Methods
  public void OnMove(InputAction.CallbackContext context)
  {
    move = context.ReadValue<Vector2>();
  }

  public void OnJump(InputAction.CallbackContext context)
  {
    if (context.started)
      jump = true;
  }

  public void OnSprint(InputAction.CallbackContext context)
  {
    if(context.started)
      sprint = true;
  }

  public void OnReset(InputAction.CallbackContext context)
  {
    reset = true;
  }

  public void OnPrimaryAbility(InputAction.CallbackContext context)
  {
    //TODO something
  }
}
