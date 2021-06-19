using UnityEngine;
using UnityEngine.UI;

namespace project_usomk
{
    public class StaminaBar : MonoBehaviour //stamina ve healthbaricin ayný sey kullanilabilir
    {
        public Slider staminaSlider;

        private void Awake()
        {
            staminaSlider = GetComponent<Slider>();
        }

        public void SetMaxStamina(int maxStamina)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }

        public void SetCurrentStamina(int currentStamina)
        {
            staminaSlider.value = currentStamina;
        }

    }
}
