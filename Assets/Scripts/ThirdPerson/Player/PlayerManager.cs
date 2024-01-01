using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Character;
using ThirdPerson.Player;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Classes")] 
    [SerializeField] private CameraHandler cameraHandler;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private PlayerLocomotion playerLocomotion;
    [SerializeField] private InteractableUI interactableUI;
    [SerializeField] private PlayerAnimationManager playerAnimationManager;
    [Header("Components")] [SerializeField]
    private Animator animator;

    public GameObject interactableUIGameObject;
    public GameObject itemInteractableUIGameObject;
    
    [SerializeField] private bool isInteracting;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isInAir;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDoCombo;

    public bool isInvulnerable;
    
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    #region Props

    public bool IsInteracting
    {
        get => isInteracting;
        set => isInteracting = value;
    }

    public bool IsSprinting
    {
        get => isSprinting;
        set => isSprinting = value;
    }

    public bool IsInAir
    {
        get => isInAir;
        set => isInAir = value;
    }

    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    public bool CanDoCombo
    {
        get => canDoCombo;
        set => canDoCombo = value;
    }

    #endregion


    private PlayerStats _playerStats;
    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        _playerStats = GetComponent<PlayerStats>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        
        isInteracting = animator.GetBool("isInteracting");
        canDoCombo = animator.GetBool("canDoCombo");
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        isUsingLeftHand = animator.GetBool("isUsingLeftHand");
        isInvulnerable = animator.GetBool("isInvulnerable");
        animator.SetBool("isDead", _playerStats.isDead);
        animator.SetBool("isInAir", isInAir);
        inputHandler.TickInput(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerLocomotion.HandleJumping();
        _playerStats.RegenerateStamina();
        
        
        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion. HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.MoveDirection);
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        //inputHandler.sprintFlag = false;
        inputHandler.lt_Input = false;
        inputHandler.rt_Input = false;
        inputHandler.rb_Input = false;
        inputHandler.d_Pad_Up = false;
        inputHandler.d_Pad_Right = false;
        inputHandler.d_Pad_Down = false;
        inputHandler.d_Pad_Left = false;
        inputHandler.f_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.inventory_Input = false;

        float delta = Time.deltaTime;
        
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
        }

        if (isInAir)
        {
            playerLocomotion.InAirTimer = playerLocomotion.InAirTimer + Time.deltaTime;
        }
    }

    #region Player Interactions

    
    private void CheckForInteractableObject()
    { 
        Vector3 rayOrigin = transform.position;
        rayOrigin.y = rayOrigin.y + 2f;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 1f, cameraHandler.IgnoreMask))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
                    string interactableText = interactableObject.InteractableText;
                    interactableUI.InteractableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.f_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }

            if (itemInteractableUIGameObject != null && inputHandler.f_Input)
            {
                itemInteractableUIGameObject.SetActive(false);
            }
        }
    }

    public void OpenChestInteraction(Transform playerStands)
    {
        playerLocomotion.Rigidbody.velocity = Vector3.zero;
        transform.position = playerStands.transform.position;
        playerAnimationManager.PlayTargetAnimation("Open Chest", true);
    }
    
    #endregion

    
}