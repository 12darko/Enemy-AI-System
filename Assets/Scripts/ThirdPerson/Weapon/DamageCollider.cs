using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Character;
using ThirdPerson.Player;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private Collider _damageCollider;
    private CharacterManager _characterManager;
    public int currentWeaponDamage = 20;
    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;

    }

    public void EnableDamageCollider()
    {
        _damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        _damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            var playerStats = other.GetComponent<PlayerStats>();
            var enemyCharacterManager = other.GetComponent<CharacterManager>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }
            
            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            var enemyStats = other.GetComponent<EnemyStats>();
            var enemyCharacterManager = other.GetComponent<CharacterManager>();
            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    _characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                    return;
                }
            }
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }
    }
}