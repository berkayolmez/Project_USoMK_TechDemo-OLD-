using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
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
            damageCollider.enabled = false; //
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            //buraya interface gelecek damageable=other.getcomponent<IDamageable>(); vs vs vs

            IDamageable damageableObj = other.GetComponent<IDamageable>();

            if(damageableObj!=null)
            {
                damageableObj.TakeDamage(currentWeaponDamage);
            }
        }
    }
}