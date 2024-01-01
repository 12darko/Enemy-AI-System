using UnityEngine;
using UnityEngine.Events;

namespace ThirdPerson.ThirdEnemy.States
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public PatrolState patrolState;
        public ReturningState returningState;
        public  bool isIdleTransition;

        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager)
        {   
            enemyManager.enemyRigidbody.isKinematic = false;
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward,Vector3.up);
            HandleRotateTowardsTarget(enemyManager);
            enemyManager.distanceFromTarget = distanceFromTarget;
            enemyManager.viewableAngle = viewableAngle;
 

            if (viewableAngle >= 45  ||  viewableAngle <= -45)
                return rotateTowardsTargetState;
            
            
            if (enemyManager.isInteracting)
                return this;
            
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
            

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            //Değişicek bu kısım idareten burada enemy  player dan ne kadar uzaksa patrola geri dönüyor 
            if (distanceFromTarget > 8f &&  isIdleTransition == false)
            {
                enemyManager.currentTarget = null;
                return patrolState;
            }

            if (distanceFromTarget > 8f && isIdleTransition)
            {
                enemyManager.currentTarget = null;
                return returningState;
            }
            

            if (distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceState;//combat stance
            }
            else
            {
                return this;
            }
          
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
            //Rotate path find with navmesh
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
                    enemyManager.rotSpeed  / Time.deltaTime );
               
            }
        }
    }
}