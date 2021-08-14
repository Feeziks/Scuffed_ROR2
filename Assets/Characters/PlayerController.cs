using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  //Public Members
  public CharacterController controller;
  public SO_CharacterData characterData;
  public InputHandler inputs;

  //Private Members
  //Dictionary<ItemID, int> itemInventory;

  //Jumping / grounded
  private int availableJumps;
  private float jumpTimer;
  private float minTimeBetweenJumps = 0.5f;
  private bool grounded;
  private float groundedOffset = -0.5f;
  private float groundedRadius = 1f;
  public LayerMask groundLayers;

  //Velocities
  private float verticalVelocity;
  private float gravity = -20f;
  private float terminalVelocity = -50.0f;

  //Health / status
  private float currHealth;
  private Constants.Status status;

  //Public Methods

  //Private Methods
  private void Awake()
  {
    jumpTimer = Time.realtimeSinceStartup;
  }

  private void Start()
  {
    
  }

  private void Update()
  {
    
  }

  private void FixedUpdate()
  {
    GroundedCheck();
    Gravity();
    Jump();
    Move();
  }

  private void LateUpdate()
  {
    //CameraRotation();
  }

  private void Jump()
  {
    if(grounded)
    {
      //Fix infinite neg vel while grounded by clamping
      if(verticalVelocity < 0.0f)
      {
        verticalVelocity = -2.0f;
      }
    }

    //Check if the character controller wants us to jump
    if (inputs.jump && (Time.realtimeSinceStartup - jumpTimer) >= minTimeBetweenJumps && availableJumps > 0)
    {
      jumpTimer = Time.realtimeSinceStartup;

      //Apply Velocity to reach the jump height && cancel any neg velocity we had before
      verticalVelocity = 0f;
      verticalVelocity = Mathf.Sqrt(characterData.characterSettings.baseJumpHeight * -2f * gravity);
      availableJumps -= 1;
      inputs.jump = false;
    }
    else
    {
      inputs.jump = false;
    }
  }

  private void Gravity()
  {
    if (verticalVelocity >= terminalVelocity)
    {
      verticalVelocity += gravity * Time.fixedDeltaTime;
    }
  }

  private void GroundedCheck()
  {
    //Cast sphere at player location to check if it clips the ground layers
    Vector3 sphereCenterPosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
    grounded = Physics.CheckSphere(sphereCenterPosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    Collider[] temp = new Collider[10];
    int asdf = Physics.OverlapSphereNonAlloc(sphereCenterPosition, groundedRadius, temp, groundLayers, QueryTriggerInteraction.Ignore);

    if (asdf != 0)
      grounded = true;

    if(grounded)
    {
      availableJumps = characterData.characterSettings.baseNumberJumps;
    }
  }

  private void Move()
  {
    controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.fixedDeltaTime);
  }
}
