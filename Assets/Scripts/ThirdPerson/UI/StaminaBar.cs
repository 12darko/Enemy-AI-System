using UnityEngine;
using UnityEngine.UI;

namespace ThirdPerson.UI
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider Slider;

        public void SetMaxStamina(float maxStamina)
        {
            Slider.maxValue = maxStamina;
            Slider.value = maxStamina;
        }
        
        public void SetCurrentStamina(float stamina)
        {
            Slider.value = stamina;
        }
    }
}