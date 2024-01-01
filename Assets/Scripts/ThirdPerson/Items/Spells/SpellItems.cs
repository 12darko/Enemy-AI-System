using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Weapon;
using UnityEngine;

public class SpellItems : Item
{
    public GameObject spellWarmUpFx;
    public GameObject spellCastFx;
    public string spellAnimation;

    [Header("Spell Cost")] public int focusPointCost;
    
    [Header("Spell Type")] public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")] [TextArea]
    public string spellDescription;


    public virtual void AttemptToCastSpell(PlayerAnimationManager playerAnimationManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
        Debug.Log("You Attempt To Cast a spell");
    }

    public virtual void SuccessfullyCastSpell(PlayerAnimationManager playerAnimationManager,PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
        Debug.Log("You Sucessfully cast a spell");
        playerStats.DeductFocusPoints(focusPointCost);
       
    }

   
}
