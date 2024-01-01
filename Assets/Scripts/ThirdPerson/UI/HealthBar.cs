using UnityEngine;
using UnityEngine.UI;

namespace ThirdPerson.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider Slider;

        public void SetMaxHealth(int maxHealth)
        {
            Slider.maxValue = maxHealth;
            Slider.value = maxHealth;
        }
        
        public void SetCurrentHealth(int health)
        {
            Slider.value = health;
        }
    }
}