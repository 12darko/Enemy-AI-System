using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Weapon;
using UnityEngine;

public class ProjectileSpell : SpellItems
{
    public float baseDamage;
    public float projectileVelocity;
    private Rigidbody rigidbody;


    public override void AttemptToCastSpell(PlayerAnimationManager playerAnimationManager, PlayerStats playerStats,WeaponSlotManager weaponSlotManager)
    {
        base.AttemptToCastSpell(playerAnimationManager, playerStats,weaponSlotManager);
    }

    public override void SuccessfullyCastSpell(PlayerAnimationManager playerAnimationManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
        base.SuccessfullyCastSpell(playerAnimationManager, playerStats,weaponSlotManager);
    }
}
