using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Player;
using ThirdPerson.Weapon;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [SerializeField] private PlayerAttacker playerAttacker;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CameraHandler cameraManager;
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float moveAmount;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    
    
    public bool b_input;
    public bool rb_Input;
    public bool rt_Input;
    public bool lt_Input;
    public bool y_Input;
    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;
    public bool f_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOnInput;
    public bool critical_attack_Input;
    
    public bool rollFlag;
    public bool sprintFlag;
    public bool twoHandFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public float rollInputTimer;
    public bool lockOnFlag;
    public bool rightStickRightInput;
    public bool rightStickLeftInput;
    
    private PlayerInputs inputActions;
    private Vector2 movementInput;
    private Vector2 cameraInput;

    private WeaponSlotManager _weaponSlotManager;
    private PlayerAnimationManager _playerAnimationManager;
    private PlayerStats _playerStats;
    public Transform criticalAttackRayCastStartPoint;

    #region Props

    public float Horizontal
    {
        get => horizontal;
        set => horizontal = value;
    }

    public float Vertical
    {
        get => vertical;
        set => vertical = value;
    }

    public float MoveAmount
    {
        get => moveAmount;
        set => moveAmount = value;
    }

    public float MouseX
    {
        get => mouseX;
        set => mouseX = value;
    }

    public float MouseY
    {
        get => mouseY;
        set => mouseY = value;
    }

    public PlayerInputs İnputActions
    {
        get => inputActions;
        set => inputActions = value;
    }

    public Vector2 MovementInput
    {
        get => movementInput;
        set => movementInput = value;
    }

    public Vector2 CameraInput
    {
        get => cameraInput;
        set => cameraInput = value;
    }

    #endregion

    private void Awake()
    {
        _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        _playerAnimationManager = GetComponentInChildren<PlayerAnimationManager>();
        _playerStats = GetComponent<PlayerStats>();
    }


    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputs();
            inputActions.PlayerMovement.Movement.performed +=
                playerInputs => movementInput = playerInputs.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            İnputActions.PlayerActions.RB.performed += i => rb_Input = true;
            İnputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerActions.LT.performed += i => lt_Input = true;
            inputActions.PlayerQuickSlot.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_Pad_Left = true;
            inputActions.PlayerActions.F.performed += i => f_Input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => rightStickLeftInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => rightStickRightInput = true;
            inputActions.PlayerActions.Y.performed += i => y_Input = true;
            inputActions.PlayerActions.CriticalAttack.performed += i => critical_attack_Input = true;
            inputActions.PlayerActions.Roll.performed += i => b_input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_input = false;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
  
        if (b_input)
        {
            rollInputTimer += delta;

            if (_playerStats.currentStamina <= 0)
            {
                b_input = false;
                sprintFlag = false;
            }

            if (moveAmount > 0.5f && _playerStats.currentStamina  > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;
            
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput(float delta)
    {
        //Hafif sağ vuruş
        if (rb_Input)
        {
            playerAttacker.HandleRBAction();
        }
        if (rt_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);   
        }

        if (lt_Input)
        {
            if (twoHandFlag)
            {
                
            }
            else
            {
             playerAttacker.HandleLTAction();   
            }
 
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }else if (d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }
    
    private void HandleInventoryInput()
    {
        
        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;
            if (inventoryFlag)
            {
                UIManager.Instance.OpenSelectWindows();
                UIManager.Instance.UpdateUI();
                UIManager.Instance.hudWindows.SetActive(false);
            }
            else
            {
                UIManager.Instance.CloseSelectWindows();
                UIManager.Instance.CloseAllInventoryWindows();
                UIManager.Instance.hudWindows.SetActive(true);

            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput  && lockOnFlag == false )
        {
            lockOnInput = false;
            lockOnFlag = true;
            cameraManager.HandleLockOn();
            if (cameraManager.nearestLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }else if (lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            cameraManager.ClearLockOnTargets();
        }

        if (lockOnFlag && rightStickLeftInput)
        {
            rightStickLeftInput = false;
            cameraManager.HandleLockOn();
            if (cameraManager.leftLockTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.leftLockTarget;
            }
        }

        if (lockOnFlag && rightStickRightInput)
        {
            rightStickRightInput = false;
            cameraManager.HandleLockOn();
            if (cameraManager.currentLockOnTarget != null)
            {
                cameraManager.currentLockOnTarget = cameraManager.rightLockTarget;
            }
        }
    }

    private void HandleTwoHandInput()
    {
        if (y_Input)
        {
            y_Input = false;
            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                _weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                _weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                _weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (critical_attack_Input)
        {
            critical_attack_Input = false;
            playerAttacker.AttemptBackStabOrRiposte();
        }
    }

}