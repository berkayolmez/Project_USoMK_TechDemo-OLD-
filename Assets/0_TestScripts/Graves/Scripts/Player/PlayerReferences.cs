using UnityEngine;

namespace project_WAST
{
    public class PlayerReferences : MonoBehaviour,IDamageable
    {
        PlayerAnimatorManager animatorManager;

        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public HealthBar healthBar;

        private void Awake()
        {
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {
            Debug.Log(healthBar);
            //healthBar = healthBar.GetComponent<HealthBar>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);
            animatorManager.PlayTargetAnimation("TakeDamage_1", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorManager.PlayTargetAnimation("Dying_1", true);
                //HandlePlayerDeath
            }
        }
    }
}