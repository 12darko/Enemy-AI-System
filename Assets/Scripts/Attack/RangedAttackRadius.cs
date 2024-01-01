using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class RangedAttackRadius : AttackRadius
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Vector3 bulletSpawnOffset;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] private GameObject muzzleObject;
    [SerializeField] private float sphereCastRadius = 0.1f;
    
    private ObjectPool _bulletPool;
    private RaycastHit _hit;
    private IDamageable _targetDamageable;
    private Bullet _bullet;

    private PoolableObject _poolableObject;
   
    protected override void Awake()
    {
        base.Awake();
        _bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackDelay) * bulletPrefab.bulletAutoDestroyTime), muzzleObject.transform.position);
    }

 

    protected override IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(attackDelay);
        yield return wait;
      
        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++)
            {
                if (HasLineOfSightTo(Damageables[i].GetTransform()))
                {
                    _targetDamageable = Damageables[i];
                    OnAttack?.Invoke(Damageables[i]);
                    enemyData.EnemyAgent.enabled = false;
                    break;
                }
            }

            if (_targetDamageable != null)
            {
                _poolableObject = _bulletPool.GetObject();
                if (_poolableObject != null)
                {
                    _bullet = _poolableObject.GetComponent<Bullet>();
                    _bullet.bulletDamage = damage;
                    _bullet.transform.position =  muzzleObject.transform.position + bulletSpawnOffset;
                    _bullet.transform.rotation = enemyData.EnemyAgent.transform.rotation;
                    _bullet.bulletRb.AddForce(enemyData.EnemyAgent.transform.forward * bulletPrefab.bulletMoveSpeed, ForceMode.VelocityChange);
                }
            }
            else
            {
                enemyData.EnemyAgent.enabled = true;
            }

            yield return wait;
            if (_targetDamageable == null || !HasLineOfSightTo(_targetDamageable.GetTransform()))
            {
                enemyData.EnemyAgent.enabled = true;
            }

            Damageables.RemoveAll(DisabledDamageables);
        }
        enemyData.EnemyAgent.enabled = false;
        AttackCoroutine = null;
    }

    private bool HasLineOfSightTo(Transform target)
    {
        if (Physics.SphereCast(transform.position + bulletSpawnOffset, sphereCastRadius, ((target.position + bulletSpawnOffset) - (transform.position + bulletSpawnOffset)).normalized, out _hit, sphereCollider.radius, bulletMask))
        {
            IDamageable damageable;
            if (_hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == target;
            }
        }

        return false;
    }
    
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (AttackCoroutine == null)
        {
            enemyData.EnemyAgent.enabled = true;
        }
    }
}
