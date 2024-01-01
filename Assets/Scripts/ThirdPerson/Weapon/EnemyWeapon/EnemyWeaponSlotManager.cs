using System;
using UnityEngine;

namespace ThirdPerson.Weapon.EnemyWeapon
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {

        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;
        
        private WeaponHolderSlot _rightHandSlot;
        private WeaponHolderSlot _leftHandSlot;

        private DamageCollider _leftHandDamageCollider;
        private DamageCollider _rightHandDamageCollider;

        private void Awake()
        {
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
            }
        }

        private void Start()
        {
            LoadWeaponsOnBothHands();
        }

        public void LoadWeaponOnSlot(WeaponItem  weapon, bool isLeft)
        {
            if (isLeft)
            {
                _leftHandSlot.currentWeapon = weapon;
                _leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                _rightHandSlot.currentWeapon = weapon;
                _rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }


        public void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }

            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }
        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                _leftHandDamageCollider = _leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                _rightHandDamageCollider = _rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void OpenDamageCollider()
        {
            _rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            _rightHandDamageCollider.DisableDamageCollider();
        }
       
        
        #region Handle's Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
           
        }

        public void DrainStaminaHeavyAttack()
        {
             
        }
        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
           // anim.SetBool("canDoCombo", false);
        }
        #endregion
    }
}