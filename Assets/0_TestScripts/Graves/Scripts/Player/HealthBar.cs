using UnityEngine;
using UnityEngine.UI;

namespace project_WAST
{ 
    public class HealthBar : MonoBehaviour
    {
        public Slider healthSlider;

        private void Awake()
        {
            healthSlider = GetComponent<Slider>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        public void SetCurrentHealth(int currentHealth)
        {
            healthSlider.value = currentHealth;
        }

    }
}