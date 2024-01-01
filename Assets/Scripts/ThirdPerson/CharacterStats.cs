using UnityEngine;

namespace ThirdPerson
{
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField]  protected int healthLevel = 10;
        [SerializeField]  protected int maxHealth;
        [SerializeField]  protected int currentHealth;
        
        [SerializeField]  protected int  staminaLevel = 10;
        [SerializeField]  protected float maxStamina;
         public float  currentStamina;

        [SerializeField] protected int focusLevel = 10;
        [SerializeField] protected float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;
        
        
        public bool isDead;
    }
}