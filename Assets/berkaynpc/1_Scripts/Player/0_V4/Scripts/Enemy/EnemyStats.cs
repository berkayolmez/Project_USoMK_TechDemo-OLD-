using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    //public class EnemyStats : CharacterStats, IDamageable
    public class EnemyStats : MonoBehaviour,IDamageable
    {
        public int healthLevel = 10; //sil
        public int maxHealth; //sil
        public int currentHealth; //sil
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealth();
            currentHealth = maxHealth;
        }

        private int SetMaxHealth()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            animator.Play("Damage_01");

            if(currentHealth<=0)
            {
                currentHealth = 0;
             animator.Play("Dead_01");
               //Handleplayerdeath

            }
        }

    }
}