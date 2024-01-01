using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using ThirdPerson.Character;
using ThirdPerson.ThirdEnemy;
using ThirdPerson.ThirdEnemy.States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class EnemyManager : CharacterManager
{
    [Header("Classes")] 
    private EnemyLocomotionManager _enemyLocomotionManager;
    private EnemyAnimatorManager _enemyAnimatorManager;
    private EnemyStats _enemyStats;
    private EnemyStats _stats;
    public Vector3 idleStartPosition;
    public Quaternion idleStartRotation;
 
 
    public Rigidbody enemyRigidbody;
    public CharacterStats currentTarget;
    public State currentState;
    public NavMeshAgent navMeshAgent;


    [Header("Combat Flags")] public bool canDoCombo;
    
    [Header("AI Settings")]
    public float detectionRadius;

    public LayerMask detectionLayer;

    //FİELD OF VIEW SETTINGS
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;
    public float viewableAngle;

    public bool isInteracting;
    public bool isPerformingAction;
    public float currentRecoveryTime = 0;
    public float distanceFromTarget;
     public float rotSpeed = 20;
    public float maximumAggroRadius = 1.5f;

    [Header("AI Combat Settings")] public bool allowAIToPerformCombos;
    public float comboLikelyHood;
    
    public bool IsPerformingAction => isPerformingAction;

    private void Awake()
    {
        _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        _stats = GetComponent<EnemyStats>();
        _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        _enemyStats = GetComponentInChildren<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>(); 
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
        idleStartPosition = navMeshAgent.transform.position;
        idleStartRotation = navMeshAgent.transform.rotation;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();
        isRotatingWithRootMotion = _enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
        isInteracting = _enemyAnimatorManager.anim.GetBool("isInteracting");
        canDoCombo = _enemyAnimatorManager.anim.GetBool("canDoCombo");
        canRotate = _enemyAnimatorManager.anim.GetBool("canRotate");
        _enemyAnimatorManager.anim.SetBool("isDead", _stats.isDead);
       // idleStartPosition.y = navMeshAgent.transform.position.y;
    }

    private void LateUpdate()
    {
         navMeshAgent.transform.localPosition = Vector3.zero;
         navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if(_enemyStats.isDead) // Ölü Düşman Kontrolü
            return;
        
        if (currentState != null)
        {
            State nextState = currentState.OnUpdate(this, _stats, _enemyAnimatorManager);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }
    
    private void SwitchToNextState(State state)
    {
        currentState = state;
    }
    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

}