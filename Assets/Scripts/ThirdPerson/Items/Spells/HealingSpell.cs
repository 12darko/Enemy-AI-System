using ThirdPerson.Weapon;
using UnityEngine;

namespace ThirdPerson.Items.Spells
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItems
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimationManager playerAnimationManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(playerAnimationManager,playerStats,weaponSlotManager);
            GameObject instantiatedWarmUpSpellFx = Instantiate(spellWarmUpFx, playerAnimationManager.transform);
            playerAnimationManager.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
            Destroy(instantiatedWarmUpSpellFx, 2.0f);
        }

        public override void SuccessfullyCastSpell(PlayerAnimationManager playerAnimationManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(playerAnimationManager, playerStats,weaponSlotManager);
            GameObject instantiatedSpellFx = Instantiate(spellCastFx, playerAnimationManager.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell Cast Successful");
            Destroy(instantiatedSpellFx, 2.0f);
          
        }
    }
}