using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider() //Set with animation events
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()  
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageableObj = other.GetComponent<IDamageable>();

            if(damageableObj!=null && !other.CompareTag("Player"))
            {
                damageableObj.TakeDamage(currentWeaponDamage);
            }
        }
    }
}