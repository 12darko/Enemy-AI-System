using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
 

public class EnemyPatrol : MonoBehaviour
{
 [SerializeField] private EnemyData enemyData;
    
    public IEnumerator DoPatrolMotion()
    {
        if (enemyData.EnemyIsAlive)
        {
            WaitForSeconds wait = new WaitForSeconds(enemyData.UpdateRate);

            yield return new WaitUntil(() => enemyData.EnemyAgent.enabled && enemyData.EnemyAgent.isOnNavMesh);
            enemyData.EnemyAgent.SetDestination(enemyData.WayPoints[enemyData.WayPointsIndex]);


            while (true)
            {
                if (enemyData.EnemyAgent.isOnNavMesh && enemyData.EnemyAgent.enabled &&
                    enemyData.EnemyAgent.remainingDistance <= enemyData.EnemyAgent.stoppingDistance)
                {
                    enemyData.WayPointsIndex++;

                    if (enemyData.WayPointsIndex >= enemyData.WayPoints.Length)
                    {
                        enemyData.WayPointsIndex = 0;
                    }

                    enemyData.EnemyAgent.SetDestination(enemyData.WayPoints[enemyData.WayPointsIndex]);
                }

                yield return wait;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < enemyData.WayPoints.Length; i++)
        {
            Gizmos.color = Color.red;;
            Gizmos.DrawWireSphere(enemyData.WayPoints[i], 0.25f);
            if (i + 1 < enemyData.WayPoints.Length)
            {
                Gizmos.DrawLine(enemyData.WayPoints[i], enemyData.WayPoints[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(enemyData.WayPoints[i], enemyData.WayPoints[0]);
            }
        }
    }
}