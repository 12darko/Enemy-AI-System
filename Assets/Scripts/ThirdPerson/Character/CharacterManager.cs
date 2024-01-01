using UnityEngine;

namespace ThirdPerson.Character
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;
        [Header("Combat Collider")]
       // public BoxCollider backStabBoxCollider;
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")] 
        public bool canBeRiposted;
        public bool canBeParried;
        public bool isParrying;

        [Header("Movement Flags")]
        
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        
        public int pendingCriticalDamage;

        public string charName;
    }
}