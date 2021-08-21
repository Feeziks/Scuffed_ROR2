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

  //Rotations
  private float targetRotation;
  private float rotationVelocity;
  //private float rotationSmoothTime = 0.12f;

  //Camera stuff
  public GameObject cinemachineCameraTarget;
  private const float cameraMoveThreshold = 0.01f;
  private bool LockCameraPosition = false;
  private float cinemachineTargetYaw;
  private float cinemachineTargetPitch;
  private float bottomClamp = 10f;
  private float topClamp = 80f;
  private float cameraAngleOverride;

  //Health / status
  private float currHealth;
  private Constants.Status status;

  private float money = 30f;

  //Projectile Pool
  public SO_Ability primaryAbility;
  private GameObject projectPoolParent;
  private ObjectPool<GameObject> projectilePool;


  private void OnEnable()
  {
    EventManager.itemPickupEvent += OnItemPickup;
  }

  private void OnDisable()
  {
    EventManager.itemPickupEvent -= OnItemPickup;
  }


  private void Awake()
  {
    jumpTimer = Time.realtimeSinceStartup;

    groundedOffset *= transform.localScale.y / 3;
    groundedRadius *= transform.localScale.magnitude;

    Cursor.lockState = CursorLockMode.Locked;

    projectilePool = new ObjectPool<GameObject>();
    InitializeProjectilePool();
  }

  private void Start()
  {
    modSettings = new ModifiedSettings();
    SetCharacterBaseStats();
  }

  private void Update()
  {
    GroundedCheck();
    Gravity();
    Jump();
    Move();
    LockPlayerRotation();

    AbilitiesCheck();
  }

  private void FixedUpdate()
  {

  }

  private void LateUpdate()
  {
    CameraRotation();
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
      verticalVelocity = modSettings.jumpHeight / (1f / timeToJumpApex);
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
    float targetVelocity = inputs.sprint ? modSettings.moveSpeed * modSettings.sprintMultiplier : modSettings.moveSpeed;
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

  private void CameraRotation()
  {
    // if there is an input and camera position is not fixed
    if (inputs.look.sqrMagnitude >= cameraMoveThreshold && !LockCameraPosition)
    {
      cinemachineTargetYaw += inputs.look.x * Time.deltaTime;
      cinemachineTargetPitch += inputs.look.y * Time.deltaTime;
    }

    // clamp our rotations so our values are limited 360 degrees
    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

    // Cinemachine will follow this target
    //cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0.0f);
  }

  private float ClampAngle(float lfAngle, float lfMin, float lfMax)
  {
    if (lfAngle < -360f) lfAngle += 360f;
    if (lfAngle > 360f) lfAngle -= 360f;
    return Mathf.Clamp(lfAngle, lfMin, lfMax);
  }

  private void LockPlayerRotation()
  {
    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0f);
  }

  #endregion

  #region Abilities

  private void AbilitiesCheck()
  {
    PrimaryAbility();
    SecondaryAbility();
    TertiaryAbility();
    QuaternaryAbility();
  }

  private void PrimaryAbility()
  {
    if(inputs.primaryAbility)
    {
      inputs.primaryAbility = false;

      //Create our projectile and shoot it
      if(primaryAbility.projectile)
      {
        GameObject go = projectilePool.Get();
        Projectile p = go.GetComponent(typeof(Projectile)) as Projectile;
        p.Spawn(transform.position, transform.forward);
      }

    }
  }
  private void SecondaryAbility()
  {

  }

  private void TertiaryAbility()
  {

  }

  private void QuaternaryAbility()
  {

  }

  private void InitializeProjectilePool()
  {
    int count = 0;
    projectPoolParent = new GameObject("ProjectilePool");
    projectPoolParent.transform.localPosition = Vector3.zero;
    projectPoolParent.transform.localScale = Vector3.one;
    foreach (GameObject go in projectilePool.pool)
    {
      //go.transform.SetParent(transform, false);
      go.transform.parent = projectPoolParent.transform;
      go.name = "Projectile_" + count.ToString();
      count++;

      MeshFilter mf = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
      MeshRenderer mr = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
      mr.enabled = false;
      SphereCollider sc = go.AddComponent(typeof(SphereCollider)) as SphereCollider;
      sc.enabled = false;
      sc.isTrigger = true;
      sc.center = Vector3.zero;
      sc.radius = primaryAbility.abilityProjectile.colliderRadius;

      Projectile p = go.AddComponent(typeof(Projectile)) as Projectile;
      p.projectileData = primaryAbility.abilityProjectile;
      p.mf = mf;
      p.mr = mr;
      p.sc = sc;

      go.SetActive(false);
    }
  }

  #endregion

  #region Items And Item Interactions

  private void OnItemPickup(OnItemPickupDataClass data)
  {
    //Modify our player's stats to reflect new items stats if relevant
    if(data.item is SO_Item_StatModifier)
    {
      //TODO Mod settings probably needs to know how many of a certain item has already been applied since first stack and subsequent stacks have different effects
      modSettings.ApplyItem(data.item);
    }
  }

  private void SetCharacterBaseStats()
  {
    modSettings.maxHealth = characterData.characterSettings.baseHealth ;
    modSettings.healthRegen = 0f;
    modSettings.moveSpeed = characterData.characterSettings.baseMoveSpeed ;
    modSettings.sprintMultiplier = characterData.characterSettings.baseSprintSpeedMultiplier;
    modSettings.jumpHeight = characterData.characterSettings.baseJumpHeight;
    modSettings.numJumps = characterData.characterSettings.baseNumberJumps;
  }

  #endregion

  #region Helper functions


  #endregion
}
