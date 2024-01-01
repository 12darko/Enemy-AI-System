using UnityEngine;

namespace ThirdPerson.Weapon
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")] public int baseDamage;
        public int criticalDamageMultiplier = 4;
        
        [Header("Idle Animation")] 
        public string right_Hand_Idle;
        public string left_Hand_Idle;
        public string th_idle;
        
        [Header("Attack Animation")]
        public string OH_LIGHT_ATTACK_1;
        public string OH_LIGHT_ATTACK_2;
        public string OH_LIGHT_ATTACK_3;
        public string OH_LIGHT_ATTACK_4;
        
        public string OH_HEAVY_ATTACK_1;
        public string OH_HEAVY_ATTACK_2;
        public string OH_HEAVY_ATTACK_3;
        public string OH_HEAVY_ATTACK_4;

        public string TH_LIGHT_ATTACK_01;


        [Header("Weapon Art")] public string WEAPON_ART;
        

        [Header("Stamina Cost")] public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")] public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;
        public bool isShieldWeapon;
    }
}