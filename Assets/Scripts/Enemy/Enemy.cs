using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable
{
    [SerializeField] public AttackRadius attackRadius;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private EnemyScriptableObject enemyScriptableObject;
    [SerializeField] private EnemyData enemyData;
    
    [SerializeField] private int enemyHealth = 100;
    private Coroutine _enemyLookCoroutine;
    #region Props

    public EnemyMovement EnemyMovement
    {
        get => enemyMovement;
        set => enemyMovement = value;
    }

    public NavMeshAgent EnemyAgent
    {
        get => enemyAgent;
        set => enemyAgent = value;
    }

    public int EnemyHealth
    {
        get => enemyHealth;
        set => enemyHealth = value;
    }

    #endregion

    public void OnEnable()
    {
        SetupAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        enemyAgent.enabled = false;
    }


    protected virtual void SetupAgentFromConfiguration()
    {
        enemyAgent.acceleration = enemyScriptableObject.Acceleration;
        enemyAgent.angularSpeed = enemyScriptableObject.AngularSpeed;
        enemyAgent.areaMask = enemyScriptableObject.AreaMask;
        enemyAgent.avoidancePriority = enemyScriptableObject.AvoidancePriority;
        enemyAgent.baseOffset = enemyScriptableObject.BaseOffset;
        enemyAgent.height = enemyScriptableObject.Height;
        enemyAgent.obstacleAvoidanceType = enemyScriptableObject.ObstacleAvoidanceType;
        enemyAgent.radius = enemyScriptableObject.Radius;
        enemyAgent.speed = enemyScriptableObject.Speed;
        enemyAgent.stoppingDistance = enemyScriptableObject.StoppingDistance;

        enemyData.UpdateRate = enemyScriptableObject.AIUpdateInterval;
        enemyData.EnemyIsAlive = true;
        enemyHealth = enemyScriptableObject.Health;

        attackRadius.sphereCollider.radius = enemyScriptableObject.AttackRadius;
        attackRadius.attackDelay = enemyScriptableObject.AttackDelay;
        attackRadius.damage = enemyScriptableObject.Damage;
    }

    private void Awake()
    {
        attackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        if (enemyData.EnemyIsAlive)
        {
            if (!enemyScriptableObject.IsRanged)
            {
                enemyAnimator.SetTrigger(EnemyConstData.AttackTrigger);
                enemyAnimator.SetBool(EnemyConstData.IsRunning, false);
                enemyAnimator.SetBool(EnemyConstData.IsWalking, false);

            }
            else
            {
                enemyAnimator.SetTrigger(EnemyConstData.RangedAttackTrigger);
                enemyAnimator.SetBool(EnemyConstData.IsRunning, false);
                enemyAnimator.SetBool(EnemyConstData.IsWalking, false);
            }
        
            if (_enemyLookCoroutine != null)
            {
                StopCoroutine(_enemyLookCoroutine);
            }

            _enemyLookCoroutine = StartCoroutine(LookAt(target.GetTransform())); 
        }
    }

    private IEnumerator LookAt(Transform target)
    {
        var lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }


    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            enemyData.EnemyIsAlive = false;
            enemyData.EnemyAnimator.SetBool(EnemyConstData.Die, true);
            attackRadius.gameObject.SetActive(false);
            // gameObject.SetActive(false);
        }
        else
        {
            //Player Her vurduğunda attack radiustaki damage yeme tipi değişicek ve bu ai da kendi damage tipinde animasyon gerçekleştiricek
            switch (attackRadius.animType)
            {
                case DamageAnimationType.Base:
                    enemyData.EnemyAnimator.SetTrigger(EnemyConstData.Hit); 
                    break;
                case DamageAnimationType.Right:
                    enemyData.EnemyAnimator.SetTrigger(EnemyConstData.Hit2); 
                    break;
                case DamageAnimationType.Left:
                    break;
                default:
                    enemyData.EnemyAnimator.SetTrigger(EnemyConstData.Hit);
                    break;
            }

        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}