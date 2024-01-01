using System;
using ThirdPerson.ThirdEnemy.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ThirdPerson.ThirdEnemy
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        private EnemyManager _enemyManager;
        private EnemyAnimatorManager _enemyAnimatorManager;
         private PatrolState _patrolState;
            
        private NavMeshTriangulation _triangulation;


        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;
 
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            characterCollider = GetComponent<CapsuleCollider>();
            _patrolState = GetComponentInChildren<PatrolState>();
        }

        private void Start()
        { 
            _triangulation = NavMesh.CalculateTriangulation();
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider,true);
            EnemyPositionSpawn();
        }

 
        private void EnemyPositionSpawn()
        {
            for (int i = 0; i <  _patrolState.WayPoints.Length -1; i++)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(_triangulation.vertices[Random.Range(0, _triangulation.vertices.Length)], out hit, 2f,   _enemyManager.navMeshAgent.areaMask))
                {
                    _patrolState.WayPoints[i] = hit.position;
                }
                else
                {
                    Debug.Log("Poziyon vertexi bulunamadı");
                }
            
            }
        }
    }
}