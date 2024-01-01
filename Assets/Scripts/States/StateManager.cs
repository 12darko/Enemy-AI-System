using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using States;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private EnemyStates defaultState;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyLineOfSightChecker lineOfSightChecker;
    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private EnemyIdle enemyIdle;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyReturning enemyReturning;
    [SerializeField] private StatesData statesData;

    public EnemyData EnemyData => enemyData;
    public StatesData StatesData => statesData;

    public EnemyStates DefaultState => defaultState;

    private void Awake()
    {
        statesData.OnStateChange += HandleStateChange;

        lineOfSightChecker.OnGainSight += HandleGainSight;
        lineOfSightChecker.OnLoseSight += HandleLoseSight;
    }


    private void HandleStateChange(EnemyStates oldState, EnemyStates newState)
    {
        if (oldState != newState)
        {
            if (enemyData.FollowCoroutine != null)
            {
                StopCoroutine(enemyData.FollowCoroutine);
            }

            if (oldState == EnemyStates.Idle)
            {
                enemyData.EnemyAgent.speed /= enemyData.IdleMoveSpeedMultiplier;
            }

            switch (newState)
            {
                case EnemyStates.Idle:
                    enemyData.FollowCoroutine = StartCoroutine(enemyIdle.DoIdleMotion());
                    break;
                case EnemyStates.Patrol:
                    enemyData.FollowCoroutine = StartCoroutine(enemyPatrol.DoPatrolMotion());
                    break;

                case EnemyStates.Chase:
                    enemyData.FollowCoroutine = StartCoroutine(enemyMovement.DoFollowTarget());
                    break;
                case EnemyStates.Spawn:
                    break;

                case EnemyStates.Returning:
                    enemyData.FollowCoroutine = StartCoroutine(enemyReturning.DoReturn());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    }

    private void HandleGainSight(Players player)
    {
        statesData.State = EnemyStates.Chase;
    }

    private void HandleLoseSight(Players player)
    { 
        if (statesData.State == EnemyStates.Chase)
        {
            statesData.State = defaultState;
            EnemyData.EnemyAnimator.SetBool(EnemyConstData.IsRunning,false);
        }

        if (defaultState == EnemyStates.Idle)
        {
            statesData.State = EnemyStates.Returning;
            EnemyData.EnemyAnimator.SetBool(EnemyConstData.IsRunning,false);
        }
        
    }

    private void OnDisable()
    {
        statesData.state = defaultState;
    }
}