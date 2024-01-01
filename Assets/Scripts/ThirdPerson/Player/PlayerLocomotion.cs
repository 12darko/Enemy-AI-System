using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private Transform cameraObject;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private PlayerAnimationManager playerAnimationManager;
    [SerializeField] private PlayerManager playerManager;
    [HideInInspector] public Transform myTransform;
    [SerializeField] private new Rigidbody rigidbody;
    private GameObject normalCamera;


    [Header("Ground Detect")] [SerializeField]
    private float groundDetectionRayStartPoint = 0.5f;

    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.2f;

    [SerializeField] private float inAirTimer;

    [Header("Movement Stats")] [SerializeField]
    private float movementSpeed = 5;

    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float sprintSpeed = 8;
    [SerializeField] private float fallingSpeed = 45;
    [SerializeField] private float walkingSpeed = 1;

    [Header("Stamina Costs")] 
    [SerializeField] private int rollStaminaCost = 15;
    [SerializeField] private int backStepStaminaCost = 12;
    [SerializeField] private int sprintStaminaCost = 1;
    
    

    #region Props

    public float GroundDetectionRayStartPoint
    {
        get => groundDetectionRayStartPoint;
        set => groundDetectionRayStartPoint = value;
    }

    public float MinimumDistanceNeededToBeginFall
    {
        get => minimumDistanceNeededToBeginFall;
        set => minimumDistanceNeededToBeginFall = value;
    }

    public float GroundDirectionRayDistance
    {
        get => groundDirectionRayDistance;
        set => groundDirectionRayDistance = value;
    }

    

    public float InAirTimer
    {
        get => inAirTimer;
        set => inAirTimer = value;
    }

    public float FallingSpeed
    {
        get => fallingSpeed;
        set => fallingSpeed = value;
    }

    public Rigidbody Rigidbody
    {
        get => rigidbody;
        set => rigidbody = value;
    }

    public Vector3 MoveDirection
    {
        get => moveDirection;
        set => moveDirection = value;
    }

    #endregion
    public LayerMask groundLayer;
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;


    private PlayerStats _playerStats;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        myTransform = transform;
        playerAnimationManager.Initialize();

        playerManager.IsGrounded = true;
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        //ignoreForGroundCheck = (1 << 6 |  1 << 13);
    }


    private void Update()
    {
        float delta = Time.deltaTime;
    }

    #region Movement

    private Vector3 normalVector;
    private Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.MoveAmount;
        targetDir = cameraObject.forward * inputHandler.Vertical;
        targetDir += cameraObject.right * inputHandler.Horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = myTransform.forward;
        }

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)
            return;
        if (playerManager.IsInteracting)
            return;

        moveDirection = cameraObject.forward * inputHandler.Vertical;
        moveDirection += cameraObject.right * inputHandler.Horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;
        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.MoveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.IsSprinting = true;
            moveDirection *= speed;
            _playerStats.TakeStaminaDamage(sprintStaminaCost);
        }
        else
        {
            if (inputHandler.MoveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
                playerManager.IsSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.IsSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        playerAnimationManager.UpdateAnimatorValues(inputHandler.MoveAmount, 0, playerManager.IsSprinting);
        
        if (playerAnimationManager.IsCanRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimationManager.Anim.GetBool("isInteracting"))
            return;

        //Check if we have stamina, if we do not, return
        if (_playerStats.currentStamina <= 0)
            return;
        
        
        if (inputHandler.rollFlag)
        {
            moveDirection = cameraObject.forward * inputHandler.Vertical;
            moveDirection += cameraObject.right * inputHandler.Horizontal;

            if (inputHandler.MoveAmount > 0)
            {
                playerAnimationManager.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                _playerStats.TakeStaminaDamage(rollStaminaCost);
            }
            else
            {
                playerAnimationManager.PlayTargetAnimation("BackStep", true);
                _playerStats.TakeStaminaDamage(backStepStaminaCost);
            }
        }
    }


    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.IsGrounded = false;
        RaycastHit hit;
        Vector3 origin = myTransform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.IsInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * GroundDirectionRayDistance;

        targetPosition = myTransform.position;
        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.IsGrounded = true;
            targetPosition.y = tp.y;
            if (playerManager.IsInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("şu süre kadar hvadaydın" + inAirTimer);
                    playerAnimationManager.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    playerAnimationManager.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.IsInAir = false;
            }
        }
        else
        {
            if (playerManager.IsGrounded)
            {
                playerManager.IsGrounded = false;
            }

            if (playerManager.IsInAir == false)
            {
                if (playerManager.IsInteracting == false)
                {
                    playerAnimationManager.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.IsInAir = true;
            }

            if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }

        if (playerManager.IsGrounded)
        {
            if (playerManager.IsInteracting || inputHandler.MoveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                myTransform.position = targetPosition;
            }
        }
    }


    public void HandleJumping()
    {
        if (playerManager.IsInteracting)
            return;
        if (_playerStats.currentStamina <= 0)
             return;

        if (inputHandler.jump_Input)
        {
            if (inputHandler.MoveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.Vertical;
                moveDirection += cameraObject.right * inputHandler.Horizontal;
                playerAnimationManager.PlayTargetAnimation("Jump", true);
                moveDirection.y = 0;
                Quaternion jumpRot = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRot;

            }
        }
    }

    #endregion
}