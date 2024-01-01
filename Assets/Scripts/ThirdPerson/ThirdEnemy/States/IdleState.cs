using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using ThirdPerson.ThirdEnemy;
using ThirdPerson.ThirdEnemy.States;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;

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
                float viewableAngle = Vector3.SignedAngle (targetDirection, enemyManager.transform.forward, Vector3.up);
                enemyManager.viewableAngle = viewableAngle;
                if (viewableAngle > enemyManager.minimumDetectionAngle &&
                    viewableAngle < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                    enemyStats.enemyHealthBar.enemyCanvas.SetActive(true);
                    pursueTargetState.isIdleTransition = true;
                    return pursueTargetState;
                }
                
            }
        }

        #endregion

      
        #region Switch To Next State
 
        if (enemyManager.currentTarget != null)
        {
         return pursueTargetState;
        }
        else
        {
           // StartCoroutine(LookAt(enemyManager)); look at attığından dolayı kafası karışıyor
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
           // enemyManager.enemyRigidbody.isKinematic = true;
          return this;
        }
        

        #endregion
    }


    private IEnumerator LookAt(EnemyManager enemyManager)
    {
        Quaternion targetRotation = Quaternion.LookRotation(enemyManager.idleStartRotation.eulerAngles);

        float time = 0, speed = 0.2f; // Dönme Hızı

        while (time < 1)
        {
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                time);
            time += Time.deltaTime * speed;

            yield return null;
        }
    }
}