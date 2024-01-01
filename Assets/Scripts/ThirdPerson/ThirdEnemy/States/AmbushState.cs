using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using ThirdPerson.ThirdEnemy;
using ThirdPerson.ThirdEnemy.States;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;
    

    public PursueTargetState pursueTargetState;
    public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (isSleeping && enemyManager.isInteracting == false)
        {
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region Target Detect

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyManager.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.SignedAngle (targetsDirection, enemyManager.transform.forward, Vector3.up);
                enemyManager.viewableAngle = viewableAngle;
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                    isSleeping = false;
                    
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    enemyStats.enemyHealthBar.enemyCanvas.SetActive(true);
                }
            }
        }


        #endregion

    

        #region State Change

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        

        #endregion
    }
}
