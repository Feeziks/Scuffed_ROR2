using System.Collections;
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

  public SortedDictionary<Constants.ItemID, int> itemInventory;
  public SO_Item equipment;

  private ModifiedSettings modSettings;

  //Jumping / grounded
  public LayerMask groundLayers;
  private int availableJumps;
  private float jumpTimer;
  private float minTimeBetweenJumps = 0.5f;
  private float timeToJumpApex = 0.5f;
  private bool grounded;
  private float groundedOffset = -0.15f; //TODO better way to do this other than guess and check
  private float groundedRadius = 0.8f;
  private float currJumpHeight;


  //Velocities
  private float verticalVelocity;
  private float velocity;
  public float gravity;
  public float fallingGravity = -150.0f;
  public float baseGravity = -30f;
  private float terminalVelocity = -700.0f;
  private float currWalkVelocity;
  private float currSprintVelocity;

  //Rotations
  private float targetRotation;
  private float rotationVelocity;
  //private float rotationSmoothTime = 0.12f;

  //Health / status
  private float currHealth;
  private Constants.Status status;

  //UI
  public UIManager UIManager;

  //Items
  public ItemManager itemManager;

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
    modSettings = new ModifiedSettings();
    InitializeItemInventory();
    SetCharacterBaseStats();
  }

  private void Update()
  {
    GroundedCheck();
    Gravity();
    Jump();
    Move();
  }

  private void FixedUpdate()
  {

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
      verticalVelocity = currJumpHeight / (1f / timeToJumpApex);
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
    //Cause player to fall faster when they reach the apex of the jump
    if (!grounded && verticalVelocity <= 0)
    {
      gravity = fallingGravity;
    }
    else
    {
      gravity = baseGravity;
    }

    if (verticalVelocity >= terminalVelocity)
    {
      verticalVelocity += gravity * Time.deltaTime;
      verticalVelocity += gravity * Time.deltaTime;
    }
  }

  private void GroundedCheck()
  {
    //Cast sphere at player location to check if it clips the ground layers
    Vector3 sphereCenterPosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
    grounded = Physics.CheckSphere(sphereCenterPosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    Collider[] temp = new Collider[10];
    int test = Physics.OverlapSphereNonAlloc(sphereCenterPosition, groundedRadius, temp, groundLayers, QueryTriggerInteraction.Ignore);

    if (test != 0)
      grounded = true;

    if(grounded && (Time.realtimeSinceStartup - jumpTimer) > Time.deltaTime * 5)
    {
      availableJumps = modSettings.numJumps;
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
      velocity = Mathf.Lerp(currentHorizontalVelocity, targetVelocity * inputMagnitude, Time.deltaTime * characterData.characterSettings.baseAccelerationRate);
      velocity = Mathf.Round(velocity * 1000f) / 1000f;
    }
    else
    {
      velocity = targetVelocity;
    }

    Vector3 targetDirection = new Vector3(inputs.move.x, 0.0f, inputs.move.y);

    controller.Move(targetDirection.normalized * (velocity * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
  }

  #endregion

  #region Items And Item Interactions
  public void OnItemPickup(SO_Item item)
  {
    //Add the item to our inventory
    itemInventory[item.ID] += 1;

    UIManager.SendMessage("UpdateItemDisplay");
    UpdateCharacterStatsOnItemPickup(item);
  }

  private void UpdateCharacterStatsOnItemPickup(SO_Item item)
  {
    modSettings.ApplyItem(item, itemInventory[item.ID]);

    currWalkVelocity = modSettings.moveSpeed;
    currSprintVelocity = currWalkVelocity * modSettings.sprintMultiplier;
    currJumpHeight = modSettings.jumpHeight;
  }

  private void SetCharacterBaseStats()
  {
    modSettings.maxHealth = characterData.characterSettings.baseHealth ;
    modSettings.healthRegen = 0f;
    modSettings.moveSpeed = characterData.characterSettings.baseMoveSpeed ;
    modSettings.sprintMultiplier = characterData.characterSettings.baseSprintSpeedMultiplier;
    modSettings.jumpHeight = characterData.characterSettings.baseJumpHeight;
    modSettings.numJumps = characterData.characterSettings.baseNumberJumps;

    currWalkVelocity = modSettings.moveSpeed;
    currSprintVelocity = currWalkVelocity * modSettings.sprintMultiplier;
    currJumpHeight = modSettings.jumpHeight;
  }

  #endregion

  #region Helper functions
  private void InitializeItemInventory()
  {
    //Initialize each itemID into the sorted dictionary in order, this way we can be ensured that the items always display / read out in the same order every time
    //Regardless of order that the player obtains the items

    //Initialize inventory dict for non-equipment items
    foreach(Constants.ItemRarity r in System.Enum.GetValues(typeof(Constants.ItemRarity)))
    {
      List<SO_Item> listItems = itemManager.GetAllItemsOfRarity(r);
      listItems.Sort((x, y) => x.ID.CompareTo(y.ID));

      foreach(SO_Item i in listItems)
      {
        itemInventory[i.ID] = 0;
      }
    }

    equipment = null;
  }

  #endregion
}
