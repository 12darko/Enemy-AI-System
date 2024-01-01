using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour
{
    #region Private Variables

   protected List<IDamageable> Damageables = new List<IDamageable>();
    protected Coroutine AttackCoroutine;

    #endregion

    #region Public Variables

    public int damage = 10;
    public float attackDelay = 0.5f;

    public delegate void AttackEvent(IDamageable target);

    public AttackEvent OnAttack;
    public SphereCollider sphereCollider;
    [FormerlySerializedAs("AnimType")] public DamageAnimationType animType;
    #endregion

  
    protected virtual void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Damageables.Add(damageable);
            if (AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(Attack());
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Damageables.Remove(damageable);
            if (Damageables.Count == 0)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        var wait = new WaitForSeconds(attackDelay);
        yield return wait;

        IDamageable closestDamageable = null;
        var closestDistance = float.MaxValue;
        while (Damageables.Count > 0)
        {
            for (var i = 0; i < Damageables.Count; i++)
            {
                var damageableTransform = Damageables[i].GetTransform();
                var distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                OnAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return wait;
            Damageables.RemoveAll(DisabledDamageables);
        }

        AttackCoroutine = null;
    }
    


    protected bool DisabledDamageables(IDamageable damageable)
    {
        return damageable != null && !damageable.GetTransform().gameObject.activeSelf;
    }
}