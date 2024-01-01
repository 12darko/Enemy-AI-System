using UnityEngine;

namespace ThirdPerson.ThirdEnemy.States
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public EnemyAttackAction currentAttack;

        private bool _isComboing = false;

        private bool _willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;
        

        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
                enemyManager.transform.position);
            
            RotateTowardTargetWhilstAttacking(enemyManager);
            
            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }
            
            if (_willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager,enemyManager);
            }

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager, enemyManager);
                RollForComboChance(enemyManager);
            }

            if (_willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;//Goes back up to perform the combo
            }

            return rotateTowardsTargetState;
        }


        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime; //Set cool down time
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            _willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }
   

        private void RotateTowardTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Manuel Rotate
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    enemyManager.rotSpeed / Time.deltaTime);
            }
        }


        private void RollForComboChance(EnemyManager enemyManager)
        {

            float comboChance = Random.Range(0, 100);
            if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyHood)
            {
                if (currentAttack.comboAction != null)
                {
                    _willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    _willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
              
            }  
        }
    }
}