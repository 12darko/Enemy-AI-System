using UnityEngine;

namespace ThirdPerson.ThirdEnemy.States
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction[] enemyAttack;


        private bool _randomDestinationSet = false;
        private float _verticalMovementValue = 0;
        private float _horizontalMovementValue = 0;

        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
                transform.position);
            enemyManager.distanceFromTarget = distanceFromTarget;
            enemyAnimatorManager.anim.SetFloat("Vertical", _verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction(enemyAnimatorManager);
                //Decide Circling action
            }

            HandleRotateTowardsTarget(enemyManager);
            
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                _randomDestinationSet = false;
                return attackState;
            }
            else
            {
                HandleGetNewAttack(enemyManager);
            }

            return this;
        }

        private void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            //Circle With only forward vertical movement
            //circle with running
            //circle with walking only
            WalkAroundTarget(enemyAnimatorManager);
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            //Manuel Rotate
            if (enemyManager.IsPerformingAction)
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
            //Rotate path find ile
            else
            {
                Vector3 relativeDirection =
                    transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation,
                    enemyManager.rotSpeed / Time.deltaTime);
            }
        }

        private void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            _verticalMovementValue = Random.Range(0, 1);

            if (_verticalMovementValue <= 1 && _verticalMovementValue > 0)
            {
                _verticalMovementValue = 0.5f;
            }
            else if (_verticalMovementValue >= -1 && _verticalMovementValue < 0)
            {
                _verticalMovementValue = -0.5f;
            }

            _horizontalMovementValue = Random.Range(-1, 1);

            if (_horizontalMovementValue <= 1 && _horizontalMovementValue >= 0)
            {
                _horizontalMovementValue = 0.5f;
            }
            else if (_horizontalMovementValue >= -1 && _horizontalMovementValue < 0)
            {
                _horizontalMovementValue = -0.5f;
            }
        }

        private void HandleGetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget =
                Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            enemyManager.distanceFromTarget = distanceFromTarget;
            enemyManager.viewableAngle = viewableAngle;

            var maxScore = 0;
            for (int i = 0; i < enemyAttack.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttack[i];
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttack.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttack[i];
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;
                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}