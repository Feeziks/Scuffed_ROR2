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
  private float timeToJumpApex = 0.5f;
  private bool grounded;
  private float groundedOffset = -0.15f; //TODO better way to do this other than guess and check
  private float groundedRadius = 0.8f;
  public LayerMask groundLayers;

  //Velocities
  private float verticalVelocity;
  private float gravity = -50f;
  private float terminalVelocity = -300.0f;

  //Health / status
  private float currHealth;
  private Constants.Status status;

  //Public Methods

  //Private Methods
  private void Awake()
  {
    jumpTimer = Time.realtimeSinceStartup;

    groundedOffset *= transform.localScale.y / 3;
    groundedRadius *= transform.localScale.magnitude;
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

      //Apply Velocity to reach the jump height && cancel any vertical velocity we had before
      verticalVelocity = 0f;
      //Desired height = s
      //initial velocity = u
      //v = 0
      //a = gravity
      //t = apex jump
      //s = u*t + (at^2)/2
      //s - (at^2)/2 = ut
      //(s - (at^2)/2)t = u
      //verticalVelocity = (characterData.characterSettings.baseJumpHeight - (gravity * timeToJumpApex * timeToJumpApex) / 2f) / timeToJumpApex;
      //verticalVelocity = Mathf.Sqrt(-2.0f * gravity * characterData.characterSettings.baseJumpHeight);
      verticalVelocity = characterData.characterSettings.baseJumpHeight / timeToJumpApex - ((gravity * timeToJumpApex) / 2f);
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

    if(grounded && (Time.realtimeSinceStartup - jumpTimer) > Time.fixedDeltaTime * 5)
    {
      availableJumps = characterData.characterSettings.baseNumberJumps;
    }
  }

  private void OnDrawGizmosSelected()
  {
    GroundedCheckGizmos();
  }

  private void GroundedCheckGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
  }

  private void Move()
  {
    controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.fixedDeltaTime);
  }
}
