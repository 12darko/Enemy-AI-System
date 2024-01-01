using System;
using System.Collections;
using System.Collections.Generic;
using Player.Ranged;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolableObject
{
    public float bulletAutoDestroyTime = 5f;
    public float bulletMoveSpeed = 2f;
    public int bulletDamage = 5;
    public Rigidbody bulletRb;

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CancelInvoke(BulletConstData.Disable);
        Invoke(BulletConstData.Disable, bulletAutoDestroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;

        if (other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(bulletDamage);
        }
        Disable();
    }

    private void Disable()
    {
        CancelInvoke(BulletConstData.Disable);
        bulletRb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
