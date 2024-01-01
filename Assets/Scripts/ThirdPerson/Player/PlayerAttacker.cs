using System;
using ThirdPerson.Character;
using ThirdPerson.Weapon;
using UnityEngine;

namespace ThirdPerson.Player
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] private PlayerAnimationManager animationHandler;
        [SerializeField] private InputHandler inputHandler;

        private PlayerManager _playerManager;
        private WeaponSlotManager _weaponSlotManager;
        private PlayerInventory _playerInventory;
        private PlayerStats _playerStats;
        public string lastAttack;

        public LayerMask backStabLayer;
        public LayerMask riposteLayeer;

        private void Awake()
        {
            _weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            _playerManager = GetComponentInParent<PlayerManager>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
            _playerStats = GetComponentInParent<PlayerStats>();
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            if (_playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animationHandler.Anim.SetBool("canDoCombo", false);

                if (lastAttack == weaponItem.OH_LIGHT_ATTACK_1)
                {
                    animationHandler.PlayTargetAnimation(weaponItem.OH_LIGHT_ATTACK_2, true);
                } /* else if (lastAttack == weaponItem.TH_LIGHT_ATTACK_01)
               {
                 //  animationHandler.PlayTargetAnimation(weaponItem.TH_LIGHT_ATTACK_02);
               }*/
            }
        }

        public void HandleLightAttack(WeaponItem weaponItem)
        {
            if (_playerStats.currentStamina <= 0)
                return;
            _weaponSlotManager.attackingWeapon = weaponItem;
            if (inputHandler.twoHandFlag)
            {
                animationHandler.PlayTargetAnimation(weaponItem.TH_LIGHT_ATTACK_01, true);
                lastAttack = weaponItem.TH_LIGHT_ATTACK_01;
            }
            else
            {
                animationHandler.PlayTargetAnimation(weaponItem.OH_LIGHT_ATTACK_1, true);
                lastAttack = weaponItem.OH_LIGHT_ATTACK_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weaponItem)
        {
            if (_playerStats.currentStamina <= 0)
                return;

            _weaponSlotManager.attackingWeapon = weaponItem;
            if (inputHandler.twoHandFlag)
            {
                //  animationHandler.PlayTargetAnimation(weaponItem.TH_HEAVY_ATTACK_01, false);
                // lastAttack = weaponItem.TH_HEAVY_ATTACK_01;
            }
            else
            {
                animationHandler.PlayTargetAnimation(weaponItem.OH_HEAVY_ATTACK_1, false);
                lastAttack = weaponItem.OH_HEAVY_ATTACK_1;
            }
        }

        #region Input Action

        public void HandleRBAction()
        {
            if (_playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (_playerInventory.rightWeapon.isSpellCaster || _playerInventory.rightWeapon.isFaithCaster ||
                     _playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(_playerInventory.rightWeapon);
            }
        }

        public void HandleLTAction()
        {
            if (_playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }else if (_playerInventory.leftWeapon.isMeleeWeapon)
            {
                
            }
        }

        #endregion


        #region Attack Actions

        public void AttemptBackStabOrRiposte()
        {
            if (_playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                    transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager =
                    hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = _weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    _playerManager.transform.position =
                        enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

                    Vector3 rotationDirection = _playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - _playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation =
                        Quaternion.Slerp(_playerInventory.transform.rotation, tr, 500 * Time.deltaTime);
                    _playerManager.transform.rotation = targetRotation;

                    var criticalDamage = _playerInventory.rightWeapon.criticalDamageMultiplier *
                                         rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animationHandler.PlayTargetAnimation("Backstab Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>()
                        .PlayTargetAnimation("Backstab Stabbed", true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                         transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayeer))
            {
                CharacterManager enemyCharacterManager =
                    hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = _weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    _playerManager.transform.position =
                        enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;
                    Vector3 rotationDirection = _playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - _playerInventory.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation =
                        Quaternion.Slerp(_playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    _playerManager.transform.rotation = targetRotation;

                    var criticalDamage = _playerInventory.rightWeapon.criticalDamageMultiplier *
                                         rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                    animationHandler.PlayTargetAnimation("Parry_Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>()
                        .PlayTargetAnimation("Parry_Stabbed", true);
                }
            }
        }


        private void PerformRBMeleeAction()
        {
            if (_playerManager.CanDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(_playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (_playerManager.IsInteracting)
                {
                    return;
                    ;
                }

                if (_playerManager.CanDoCombo)
                {
                    return;
                }

                animationHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(_playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (_playerManager.IsInteracting)
                return;

            if (weapon.isFaithCaster)
            {
                if (_playerInventory.currentSpell != null && _playerInventory.currentSpell.isFaithSpell)
                {
                    if (_playerStats.currentFocusPoints >= _playerInventory.currentSpell.focusPointCost)
                    {
                        _playerInventory.currentSpell.AttemptToCastSpell(animationHandler, _playerStats, _weaponSlotManager);
                    }
                    else
                    {
                        //focus points no anim
                        animationHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (_playerManager.IsInteracting)
                return;
            if (isTwoHanding)
            {
               
            }
            else
            {
                animationHandler.PlayTargetAnimation(_playerInventory.leftWeapon.WEAPON_ART, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            _playerInventory.currentSpell.SuccessfullyCastSpell(animationHandler, _playerStats, _weaponSlotManager);
        }

        #endregion
    }
}