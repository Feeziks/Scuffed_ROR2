﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  //Public Members
  public CharacterController controller;
  public SO_CharacterData characterData;
  public InputHandler inputs;
  public Camera mainCamera;

  //Private Members
  SortedDictionary<Constants.ItemID, int> itemInventory;

  //Jumping / grounded
  public LayerMask groundLayers;
  private int availableJumps;
  private float jumpTimer;
  private float minTimeBetweenJumps = 0.5f;
  private float timeToJumpApex = 0.5f;
  private bool grounded;
  private float groundedOffset = -0.15f; //TODO better way to do this other than guess and check
  private float groundedRadius = 0.8f;
  

  //Velocities
  private float verticalVelocity;
  private float velocity;
  private float gravity = -50f;
  private float terminalVelocity = -300.0f;
  private float currWalkVelocity;
  private float currSprintVelocity;

  //Rotations
  private float targetRotation;
  private float rotationVelocity;
  private float rotationSmoothTime = 0.12f;

  //Health / status
  private float currHealth;
  private Constants.Status status;

  //UI
  public GameObject spriteStorage;
  public Sprite temporary;

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
    itemInventory = new SortedDictionary<Constants.ItemID, int>();
    InitializeItemInventory();
    UpdateCharacterStats();
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

  #region Movement / Control
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
      verticalVelocity = Mathf.Sqrt(-2.0f * gravity * characterData.characterSettings.baseJumpHeight);
      //verticalVelocity = characterData.characterSettings.baseJumpHeight / timeToJumpApex - ((gravity * timeToJumpApex) / 2f);
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
    //Get target velocity based on sprinting or not
    float targetVelocity = inputs.sprint ? currSprintVelocity : currWalkVelocity;
    if (inputs.move == Vector2.zero)
      targetVelocity = 0.0f;

    float velocityOffset = 0.1f;
    float currentHorizontalVelocity = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

    float inputMagnitude = inputs.analogMovement ? inputs.move.magnitude : 1.0f;

    //Acceleration or decall to target vel
    if (currentHorizontalVelocity < targetVelocity - velocityOffset || currentHorizontalVelocity > targetVelocity + velocityOffset)
    {
      velocity = Mathf.Lerp(currentHorizontalVelocity, targetVelocity * inputMagnitude, Time.fixedDeltaTime * characterData.characterSettings.baseAccelerationRate);
      velocity = Mathf.Round(velocity * 1000f) / 1000f;
    }
    else
    {
      velocity = targetVelocity;
    }

    Vector3 inputDirection = new Vector3(inputs.move.x, 0.0f, inputs.move.y).normalized;
    
    if(inputs.move != Vector2.zero)
    {
      targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
      float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

      transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

    controller.Move(targetDirection.normalized * (velocity * Time.fixedDeltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.fixedDeltaTime);
  }

  #endregion

  #region Items And Item Interactions
  public void OnItemPickup(Constants.ItemID item)
  {
    //Add the item to our inventory
    itemInventory[item] += 1;

    UpdateItemDisplay();
    UpdateCharacterStats();
  }

  private void UpdateItemDisplay()
  {
    // Update the sprites displayed in the UI if necessary + the number of that item owned
    foreach(KeyValuePair<Constants.ItemID, int> kvp in itemInventory)
    {
      if(kvp.Value == 1)
      {
        //Display the sprite in the UI
        //TODO: Need some kind of ItemID to ItemPrefab LUT
        //TODO: Check if there is an Image component associate with this Item already? Just make sure we are erases and rebuilding this list every time we get a new item
        //Better to just check the images that are already there and update from there? idk
        spriteStorage.GetComponent<Image>().sprite = temporary;
      }
      else if(kvp.Value > 1)
      {
        //Dispaly the sprite and a number showing how many are owned
      }
    }

  }

  private void UpdateCharacterStats()
  {
    currWalkVelocity = characterData.characterSettings.baseMoveSpeed;
    currSprintVelocity = characterData.characterSettings.baseMoveSpeed * characterData.characterSettings.baseSprintSpeedMultiplier;
  }

  #endregion

  #region Helper functions
  private void InitializeItemInventory()
  {
    //Initialize each itemID into the sorted dictionary in order, this way we can be ensured that the items always display / read out in the same order every time
    //Regardless of order that the player obtains the items
    
    //TODO: Get every valid ItemID - Create an entry in this sorted dict with that ID having 0 instances
    for(int i = 0; i < Constants.startAndEndOfItemIDByRarirty.GetLength(1); i++)
    {
      for (uint j = Constants.startAndEndOfItemIDByRarirty[0,i].ID; j <= Constants.startAndEndOfItemIDByRarirty[1,i].ID; j++)
      {
        Constants.ItemID thisID = new Constants.ItemID(j);
        itemInventory[thisID] = 0;
      }
    }
  }

  #endregion
}
