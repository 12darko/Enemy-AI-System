using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThirdPerson.ThirdEnemy.States
{
    public class PatrolState : State
    {
        [SerializeField] private PursueTargetState pursueTargetState;
        [SerializeField] private RotateTowardsTargetState rotateTowardsTargetState;
        [SerializeField] private int wayPointsIndex = 0;
        [SerializeField] private Vector3[] wayPoints = new Vector3[4];
        [SerializeField] private float updateRate = 0.01f;
        public float detectionRadius = 2;

        private Vector3 _targetDirection;
        
        #region Props

        public int WayPointsIndex
        {
            get => wayPointsIndex;
            set => wayPointsIndex = value;
        }

        public Vector3[] WayPoints
        {
            get => wayPoints;
            set => wayPoints = value;
        }

        #endregion
        
        public override State OnUpdate(EnemyManager enemyManager, EnemyStats enemyStats,
            EnemyAnimatorManager enemyAnimatorManager)
        {      
        
            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                return this;
            }
            
      
            SearchForTarget(enemyManager, enemyStats);
        
            if (enemyManager.currentTarget != null)
            {
                return rotateTowardsTargetState;
            }
            else
            {
                enemyManager.navMeshAgent.enabled = true;
                StartCoroutine(HandlePatrol(enemyManager, enemyAnimatorManager));
                return this;
            }
        }

        private IEnumerator HandlePatrol(EnemyManager enemyManager, EnemyAnimatorManager enemyAnimatorManager)
        {
         
            WaitForSeconds wait = new WaitForSeconds(updateRate);

            yield return new WaitUntil(() =>
                enemyManager.navMeshAgent.enabled && enemyManager.navMeshAgent.isOnNavMesh);
            enemyAnimatorManager.anim.SetFloat("Vertical", 0.5f, 0.1f, Time.deltaTime);
            Quaternion targetRotation = Quaternion.Lerp(enemyManager.transform.rotation,
                enemyManager.navMeshAgent.transform.rotation,  enemyManager.rotSpeed  / Time.deltaTime );
            enemyManager.transform.rotation = targetRotation;
            enemyManager.navMeshAgent.SetDestination(wayPoints[wayPointsIndex]);


            while (true)
            {
                if (enemyManager.navMeshAgent.isOnNavMesh && enemyManager.navMeshAgent.enabled &&
                    enemyManager.navMeshAgent.remainingDistance <= enemyManager.navMeshAgent.stoppingDistance)
                {
                    wayPointsIndex++;

                    if (wayPointsIndex >= wayPoints.Length)
                    {
                        wayPointsIndex = 0;
                    }
                    enemyManager.navMeshAgent.SetDestination(wayPoints[wayPointsIndex]);
                }

                yield return wait;
            }
        }

        private void SearchForTarget(EnemyManager enemyManager, EnemyStats enemyStats)
        {
            #region Target Detect

            Collider[] colliders =
                Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyManager.detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.SignedAngle (targetsDirection, enemyManager.transform.forward, Vector3.up);
                    enemyManager.viewableAngle = viewableAngle;

                    _targetDirection = targetsDirection;
                    if (viewableAngle> enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        enemyStats.enemyHealthBar.enemyCanvas.SetActive(true);
                    }
                   
                }
            }

            #endregion
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < wayPoints.Length; i++)
            {
                Gizmos.color = Color.red;
                ;
                Gizmos.DrawWireSphere(wayPoints[i], 0.25f);
                if (i + 1 < wayPoints.Length)
                {
                    Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
                }
                else
                {
                    Gizmos.DrawLine(wayPoints[i], wayPoints[0]);
                }
            }
        }
    }
}