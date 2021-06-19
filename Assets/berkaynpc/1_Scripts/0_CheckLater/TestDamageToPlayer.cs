using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class TestDamageToPlayer : MonoBehaviour
    {
        public int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerReferences playerRefs = other.GetComponent<PlayerReferences>();

            if(playerRefs!=null)
            {
                playerRefs.TakeDamage(damage);
            }
        }
    }
}