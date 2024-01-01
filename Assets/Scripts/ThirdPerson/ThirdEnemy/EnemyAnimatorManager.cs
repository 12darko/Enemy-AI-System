using System;
using ThirdPerson.Player;
using UnityEngine;

namespace ThirdPerson.ThirdEnemy
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;
        private EnemyStats _enemyStats;
        
        [SerializeField] private bool isCanRotate;
        
        public bool IsCanRotate
        {
            get => isCanRotate;
            set => isCanRotate = value;
        }
        private void Awake()
        {
            anim = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyStats = GetComponentInParent<EnemyStats>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            _enemyStats.TakeDamageNoAnimation(_enemyManager.pendingCriticalDamage);
            _enemyManager.pendingCriticalDamage = 0;
        }

        //Eğer Soul çekmek istersen animator penceresinde seçtir
        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCounterBar soulCounterBar = FindObjectOfType<SoulCounterBar>();
            if (playerStats != null)
            {
                playerStats.AddSouls(_enemyStats.soulAwardedOnDeath);
            }

            if (soulCounterBar != null)
            {
                soulCounterBar.SetSoulCountText(playerStats.soulCount);
            }
        }
        
        
        public void CanRotate()
        {
            isCanRotate = true;
        }
   
        public void StopRotation()
        {
            isCanRotate = false;
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }
        
        public void EnableIsParrying()
        {
            _enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            _enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            _enemyManager.canBeRiposted = true;
        } 
        public void DisableCanBeRiposted()
        {
            _enemyManager.canBeRiposted = false;
        }
        
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPos = anim.deltaPosition;
            deltaPos.y = 0;
            Vector3 velocity = deltaPos / delta;
            _enemyManager.enemyRigidbody.velocity = velocity;

            if (_enemyManager.isRotatingWithRootMotion)
            {
                _enemyManager.transform.rotation *= anim.deltaRotation;
            }
            
        }
        
    }
}