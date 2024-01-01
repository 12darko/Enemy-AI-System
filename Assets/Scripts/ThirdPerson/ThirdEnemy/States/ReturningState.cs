using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPerson.ThirdEnemy.States
{
    public class ReturningState : State
    {
        public IdleState idleState;
        public PursueTargetState pursueTargetState;
        [SerializeField] private float updateRate = 0.01f;


        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection

            Collider[] colliders =
                Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, enemyManager.detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // Check id

                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle =
                        Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

                    if (viewableAngle > enemyManager.minimumDetectionAngle &&
                        viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        enemyStats.enemyHealthBar.enemyCanvas.SetActive(true);
                    }
                }
            }

            #endregion


            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                enemyStats.enemyHealthBar.enemyCanvas.SetActive(false);
                return HandleReturnToStandPosition(enemyManager, enemyAnimatorManager);
            }
        }


        private State HandleReturnToStandPosition(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            var distanceFromCurrentReturningPoint = Vector3.Distance(enemyManager.transform.position,
                enemyManager.idleStartPosition);
            //%80 hatasız çalışıyor ama küçüktür kullanarak denersek belki ihtimal yüzdesini arttırabiliriz
            if (distanceFromCurrentReturningPoint >= .2)
            {
                enemyManager.navMeshAgent.destination = enemyManager.idleStartPosition;
                Quaternion targetRotation = Quaternion.Lerp(enemyManager.transform.rotation,
                    enemyManager.navMeshAgent.transform.rotation, 0.5f);
                enemyManager.transform.rotation = targetRotation;
                enemyAnimatorManager.anim.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
            }
            else
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
                enemyManager.transform.rotation = enemyManager.idleStartRotation;
                return idleState;
            }

            return this;
        }
    }
}