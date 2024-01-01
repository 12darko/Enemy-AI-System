using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Player
{
    public class EnemyIdle : MonoBehaviour
    {
   [SerializeField] private EnemyData enemyData;
  
        public IEnumerator DoIdleMotion()
        {
            if (enemyData.EnemyIsAlive)
            {
                var wait = new WaitForSeconds(enemyData.UpdateRate);

                enemyData.EnemyAgent.speed *=  enemyData.IdleMoveSpeedMultiplier;

                while (true)
                {
                    if (!enemyData.EnemyAgent.enabled ||  enemyData.EnemyAgent.isOnNavMesh)
                    {
                        yield return wait;
                    }
                    else if (enemyData.EnemyAgent.remainingDistance <=  enemyData.EnemyAgent.stoppingDistance)
                    {
                        Vector2 point = Random.insideUnitCircle *  enemyData.IdleLocationRadius;
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition( enemyData.EnemyAgent.transform.position + new Vector3(point.x,0,point.y), out  hit, 2f,  enemyData.EnemyAgent.areaMask))
                        {
                            enemyData.EnemyAgent.SetDestination(hit.position);
                        }
                    }
                    yield return wait;
                }
            }
        }
    }
}