using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TestCharacter : MonoBehaviour, I_PlayableCharacter
{
  public PlayableCharacterSettings characterSettings { get ; set; }
  public InputActions inputs;
  public CharacterController controller;
  public GameObject mainCamera;
  public GameObject cinemachineCameraTarget;

  public float gravity = -10.0f;
  public float verticalVelocity;
  private float terminalVelocity = 100.0f;

  private float velocity;
  public float accelerationRate;

  private float targetRotation;
  private float rotationVelocity;
  public float rotationSmoothTime;
  public float lookThreshold;
  public float cameraAngleOverride;

  public float topClamp;
  public float bottomClamp;
  public bool lockCameraPosition;
  private float cinemachineTargetYaw;
  private float cinemachineTargetPitch;


  public bool grounded = true;
  public LayerMask groundLayers;
  public float groundedOffset = -0.14f;
  public float groundedRadius = 0.28f;

  private int availableJumps;

  private void Awake()
  {
    
  }

  private void Start()
  {
    characterSettings = new PlayableCharacterSettings();
    InitSettings();
  }

  private void FixedUpdate()
  {
    JumpAndGravity();
    GroundedCheck();
    Move();
  }

  private void LateUpdate()
  {
    CameraRotation();
  }

  private void JumpAndGravity()
  {
    if(grounded)
    {
      //On the ground  - reset number of available jumps
      availableJumps = (int)(characterSettings.baseNumberJumps + characterSettings.numberJumpsModifier);

      //Limit negative velocity so we dont fall through the floor
      if(verticalVelocity < 0.0f)
      {
        verticalVelocity = -2.0f;
      }

      //Check if we need to jump
      if(inputs.jump && availableJumps > 0)
      {
        verticalVelocity = Mathf.Sqrt((float)(characterSettings.baseJumpHeight * (1.0 + characterSettings.jumpHeightModifier) * -2f * gravity));
      }
    }
    else
    {
      //Airborne - do nothing for now I guess
      ;
    }

    //Apply gravity to our vertical velocity
    if(verticalVelocity < terminalVelocity)
    {
      verticalVelocity += gravity * Time.deltaTime;
    }
  }

  private void GroundedCheck()
  {
    //Sphere cast from character model and see if it collids with the ground
    Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
    grounded = Physics.CheckSphere(spherePos, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
  }

  private void Move()
  {
    //Set target speed
    float targetSpeed = 0.0f;
    if(inputs.sprint)
    {
      //We are sprinting
      targetSpeed = characterSettings.baseMoveSpeed * (characterSettings.baseSprintSpeedMultiplier + characterSettings.sprintSpeedModifier) * (1.0f + characterSettings.moveSpeedModifier);
    }
    else
    {
      //Walking
      targetSpeed = characterSettings.baseMoveSpeed * (1.0f + characterSettings.moveSpeedModifier);
    }

    //Make sure we dont move if there is no movement input
    if (inputs.move == Vector2.zero)
      targetSpeed = 0f;

    Debug.Log(targetSpeed);

    //Get current horizontal velocity
    float currHorizVelocity = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
    float inputMagnitude = inputs.analogMovement ? inputs.move.magnitude : 1f;
    float speedOffset = 0.1f;

    if(currHorizVelocity < targetSpeed - speedOffset || currHorizVelocity > targetSpeed + speedOffset)
    {
      velocity = Mathf.Lerp(currHorizVelocity, targetSpeed * inputMagnitude, Time.fixedDeltaTime * accelerationRate);
      //Minor rounding
      velocity = Mathf.Round(velocity * 1000f) / 1000f;
    }
    else
    {
      velocity = targetSpeed;
    }

    // normalize input direction
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


  private void CameraRotation()
  {
    if(inputs.look.sqrMagnitude >= lookThreshold && !lockCameraPosition)
    {
      cinemachineTargetYaw += inputs.look.x * Time.deltaTime;
      cinemachineTargetPitch += inputs.look.y * Time.deltaTime;
    }

    //Clamp rotations to 360
    cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
    cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

    cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0.0f);
  }

  private float ClampAngle(float lfAngle, float lfMin, float lfMax)
  {
    if (lfAngle < -360f)
      lfAngle += 360f;
    if (lfAngle > 360f)
      lfAngle -= 360f;

    return Mathf.Clamp(lfAngle, lfMin, lfMax);
  }

  public void InitSettings()
  {
    characterSettings.baseHealth = 100.0f;
    characterSettings.healthModifier = 0.0f;

    characterSettings.baseArmor = 0.0f;
    characterSettings.armorModifier = 0.0f;

    characterSettings.baseMoveSpeed = 10.0f;
    characterSettings.moveSpeedModifier = 0.0f;

    characterSettings.baseSprintSpeedMultiplier = 2.5f;
    characterSettings.sprintSpeedModifier = 0.0f;

    characterSettings.baseJumpHeight = 20.0f;
    characterSettings.jumpHeightModifier = 0.0f;

    characterSettings.baseNumberJumps = 2;
    characterSettings.numberJumpsModifier = 0;
  }

  public void OnPrimaryAbilityAction(InputAction.CallbackContext context)
  {
    throw new System.NotImplementedException();
  }

  public void OnSecondaryAbilityAction(InputAction.CallbackContext context)
  {
    throw new System.NotImplementedException();
  }

  public void RecieveDamage(float damage, Constants.ElementTypes elemType)
  {
    throw new System.NotImplementedException();
  }
}
