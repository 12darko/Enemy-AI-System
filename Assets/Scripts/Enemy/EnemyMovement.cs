using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using States;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;

    public StateManager StateManager => stateManager;

    public NavMeshTriangulation Triangulation;
 
    private void Awake()
    {
        stateManager.EnemyData.EnemyAgent = GetComponent<NavMeshAgent>();
        stateManager.EnemyData.EnemyAgentLinkMover = GetComponent<AgentLinkMover>();

        stateManager.EnemyData.EnemyAgentLinkMover.OnLinkStart += HandleLinkStart;
        stateManager.EnemyData.EnemyAgentLinkMover.OnLinkEnd += HandleLinkEnd;
    }
    
    public void StartChasing()
    {
        if ( stateManager.EnemyData.FollowCoroutine == null)
        {
            stateManager.EnemyData.FollowCoroutine = StartCoroutine(DoFollowTarget());
        }
        else
        {
            Debug.LogWarning(" Düşman üzerinde Zaten bir Chase Var");
        }
    }

    
    private void HandleLinkStart()
    {
        stateManager.EnemyData.EnemyAnimator.SetTrigger(EnemyConstData.Jump);
    }

    private void HandleLinkEnd()
    {
        stateManager.EnemyData.EnemyAnimator.SetTrigger(EnemyConstData.Landed);
    }

    private void Update()
    {
        if (stateManager.EnemyData.EnemyIsAlive)
        {
            if (stateManager.StatesData.state == EnemyStates.Chase)
            {
                stateManager.EnemyData.EnemyAnimator.SetBool(EnemyConstData.IsRunning, stateManager.EnemyData.EnemyAgent.velocity.magnitude > 0.01f);
            }
            else
            {
                stateManager.EnemyData.EnemyAnimator.SetBool(EnemyConstData.IsWalking,    stateManager.EnemyData.EnemyAgent.velocity.magnitude > 0.01f); 
            }
        }
        else
        {
            stateManager.EnemyData.EnemyAgent.isStopped = true;
        }
    }

    
    public void Spawn()
    {
        for (int i = 0; i <  stateManager.EnemyData.WayPoints.Length; i++)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(Triangulation.vertices[Random.Range(0, Triangulation.vertices.Length)], out hit, 2f,   stateManager.EnemyData.EnemyAgent.areaMask))
            {
                stateManager.EnemyData.WayPoints[i] = hit.position;
            }
            else
            {
                Debug.Log("Poziyon vertexi bulunamadı");
            }
            
        }
        stateManager.StatesData.OnStateChange?.Invoke(EnemyStates.Spawn,   stateManager.DefaultState);
    }

    
    public IEnumerator DoFollowTarget()
        {
            if (stateManager.EnemyData.EnemyIsAlive)
            {
                WaitForSeconds Wait = new WaitForSeconds(stateManager.EnemyData.UpdateRate);

                while (gameObject.activeSelf)
                {
                    if (stateManager.EnemyData.EnemyAgent.enabled)
                    {
                        stateManager.EnemyData.EnemyAgent.SetDestination(stateManager.EnemyData.PlayerTransform
                            .transform.position);
                    }

                    yield return Wait;
                }
            }
        }
}