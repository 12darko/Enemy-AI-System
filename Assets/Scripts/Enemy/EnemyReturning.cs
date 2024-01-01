using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class EnemyReturning : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;
    private void Start()
    {
        stateManager.EnemyData.IdleStartPosition =  stateManager.EnemyData.EnemyAgent.transform.position;
    }

    public IEnumerator DoReturn()
    {
        if (stateManager.EnemyData.EnemyIsAlive)
        {
            WaitForSeconds wait = new WaitForSeconds(stateManager.EnemyData.UpdateRate);

            yield return new WaitUntil(() => stateManager.EnemyData.EnemyAgent.enabled && stateManager.EnemyData.EnemyAgent.isOnNavMesh);
        
            if (stateManager.EnemyData.EnemyAgent.transform.position == stateManager.EnemyData.IdleStartPosition)
            {
                stateManager.StatesData.State = EnemyStates.Idle;
            }
            else
            {
                stateManager.StatesData.State = EnemyStates.Returning;
                stateManager.EnemyData.EnemyAgent.SetDestination(stateManager.EnemyData.IdleStartPosition);
            }
        }
    }
}
