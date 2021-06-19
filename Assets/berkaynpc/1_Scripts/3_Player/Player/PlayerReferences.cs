using UnityEngine;

namespace project_usomk
{
    public class PlayerReferences : MonoBehaviour,IDamageable
    {
        PlayerAnimatorManager animatorManager;

        [Header("Character Health")]
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        public HealthBar healthBar;
        public StaminaBar staminaBar;

        [Header("Character Stamina")]
        public int staminaLevel = 10;
        public int maxStamina;
        public int currentStamina;

        private void Awake()
        {
            //healthBar = FindObjectOfType<HealthBar>(); disaridan bulmak yerine el ile konuldu fikir degisirsen bunu ac
            //staminaBar = FindObjectOfType<staminaBar>(); disaridan bulmak yerine el ile konuldu fikir degisirsen bunu ac
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {         
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);
            if(currentHealth>0)
            {
                animatorManager.PlayTargetAnimation("TakeDamage_1", true);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorManager.PlayTargetAnimation("Dying_1", true);
                //HandlePlayerDeath
            }
        }

        #region This will be activated

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        #endregion
    }
}