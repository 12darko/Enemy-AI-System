using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThirdPerson.Weapon
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private PlayerInventory _playerInventory;
        private WeaponHolderSlot _leftHandSlot;
        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _backSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [FormerlySerializedAs("_attackingWeapon")]
        public WeaponItem attackingWeapon;

        private InputHandler _inputHandler;

        private Animator _animator;

        private PlayerStats _playerStats;

        [SerializeField] private QuickSlotsUI quickSlotsUI;

        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _animator = GetComponent<Animator>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            _playerStats = GetComponentInParent<PlayerStats>();
            _inputHandler = GetComponentInParent<InputHandler>();

            
            
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlots in weaponHolderSlots)
            {
                if (weaponSlots.isLeftHandSlot)
                {
                    _leftHandSlot = weaponSlots;
                }
                else if (weaponSlots.isRightHandSlot)
                {
                    _rightHandSlot = weaponSlots;
                }
                else if (weaponSlots.isBackSlot)
                {
                    _backSlot = weaponSlots;
                }
            }
        }


        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                _leftHandSlot.currentWeapon = weaponItem;
                _leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

                #region Handle Left Weapon Idle Animation

                if (weaponItem != null)
                {
                    //_animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
                }
                else
                {
                    _animator.CrossFade("Left Arm Empty", 0.2f);
                }

                #endregion
            }
            else
            {
                if (_inputHandler.twoHandFlag)
                {
                    _backSlot.LoadWeaponModel(_leftHandSlot.currentWeapon);
                    _leftHandSlot.UnloadWeaponAndDestroy();
                    _animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    #region Handle Right Weapon Idle Animation

                    _animator.CrossFade("Both Arm Empty", 0.2f);
                    
                    _backSlot.UnloadWeaponAndDestroy();
                    
                    if (weaponItem != null)
                    {
                        //  _animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
                    }
                    else
                    {
                        _animator.CrossFade("Right Arm Empty", 0.2f);
                    }

                    #endregion
                }

                _rightHandSlot.currentWeapon = weaponItem;
                _rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }

        #region Weapon Damage Collider Control

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = _playerInventory.leftWeapon.baseDamage;

        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = _playerInventory.rightWeapon.baseDamage;
        }

        public void OpenDamageCollider()
        {
            if (_playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();  
            }else if (_playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();  
            }
         
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }

        #endregion

        #region Handle's Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            _playerStats.TakeStaminaDamage(
                Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            _playerStats.TakeStaminaDamage(
                Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion
    }
}