using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using ThirdPerson.ThirdEnemy;
using UnityEngine;

public class EnemyStats : CharacterStats
{

  public EnemyHealthBar enemyHealthBar;
  
  public int soulAwardedOnDeath = 50;
  private EnemyAnimatorManager _enemyAnimatorManager;

  private void Awake()
  {
    _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
  }

  #region Props
    
      public int HealthLevel => healthLevel;
    
      public int MaxHealth => maxHealth;
    
      public int CurrentHealth => currentHealth;
    
      #endregion
      private void Start()
      {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
      }
    
      private int SetMaxHealthFromHealthLevel()
      {
        maxHealth = healthLevel * 10;
        return maxHealth;
      }

      public void TakeDamageNoAnimation(int damage)
      {
        currentHealth = currentHealth - damage;
        
        enemyHealthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
          currentHealth = 0;
          isDead = true;
        }
      }
      public void TakeDamage(int damage)
      {
        if (isDead)
          return;
        
        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);
        _enemyAnimatorManager.anim.SetTrigger("Hit");
    
        if (currentHealth <= 0)
        {
          HandeDeath();
        }
      }

      private void HandeDeath()
      {
        currentHealth = 0;
         _enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        isDead = true;
   
        //Scan For Every player in the scene award them souls
      }
}
