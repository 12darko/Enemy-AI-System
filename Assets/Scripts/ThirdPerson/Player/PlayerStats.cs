using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson;
using ThirdPerson.UI;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager _playerManager;
    [SerializeField] private PlayerAnimationManager playerAnimationManager;

    #region Props

    public int HealthLevel => healthLevel;

    public int MaxHealth => maxHealth;

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    #endregion

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public FocusPointBar focusPointBar;
    public float staminaRegenerationAmount = 1;
    public float staminaRegenTimer = 0;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        maxStamina = SetMaxStaminaFromStaminaLevel();
        maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
        
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentFocusPoints = maxFocusPoints;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
        
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);
        
        focusPointBar.SetMaxFocusPoint(maxFocusPoints);
        focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }
    private float SetMaxFocusPointsFromFocusLevel()
    {
        maxFocusPoints = focusLevel * 10;
        return maxFocusPoints;
    }
    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            // animatorHandler.PlayTargetAnimation("Dead_01", true);
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (_playerManager.isInvulnerable)
            return;
        if (isDead)
            return;

        currentHealth = currentHealth - damage;

        playerAnimationManager.PlayTargetAnimation("Hit_1", true);

        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // animatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
    public void RegenerateStamina()
    {
        if (_playerManager.IsInteracting)
        {
            staminaRegenTimer = 0;
        }

        else
        {
            staminaRegenTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }
    public void HealPlayer(int healAmount)
    {
       currentHealth = currentHealth + healAmount;
       if (currentHealth > maxHealth)
       {
           currentHealth = maxHealth;
       }
       
       healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductFocusPoints(int focusPoints)
    {
        currentFocusPoints = currentFocusPoints - focusPoints;
        if (currentFocusPoints < 0)
        {
            currentFocusPoints = 0;
        }
        focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
    }

    public void AddSouls(int souls)
    {
        soulCount = soulCount + souls;
    }
    
}