using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Animations;

public class Players : MonoBehaviour, IDamageable
{
    //Components
    [SerializeField] private AttackRadius attackRadius;
    [SerializeField] private Animator playerAnimator;


    //Variables
    [SerializeField] private int playerHealth = 150;

    private void Awake()
    {
        attackRadius.sphereCollider.enabled =false;
        attackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        target.GetTransform().transform.GetComponent<Enemy>().attackRadius.animType = attackRadius.animType;
    }
    
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            //Player Her vurduğunda attack radiustaki damage yeme tipi değişicek ve bu ai da kendi damage tipinde animasyon gerçekleştiricek
            switch (attackRadius.animType)
            {/*
                case DamageAnimationType.Base:
                    playerAnimator.SetTrigger(EnemyConstData.Hit); 
                    break;
                case DamageAnimationType.Right:
                    playerAnimator.SetTrigger(EnemyConstData.Hit2); 
                    break;
                case DamageAnimationType.Left:
                    break;
                default:
                    playerAnimator.SetTrigger(EnemyConstData.Hit);
                    break;*/
            }

        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}