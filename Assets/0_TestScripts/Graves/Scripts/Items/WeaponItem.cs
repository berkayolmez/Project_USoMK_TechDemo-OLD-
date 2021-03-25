using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    [CreateAssetMenu(menuName ="Items/Weapon")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attack Animations")]
        public string OH_LightAttack_1;
        public string OH_HeavyAttack_1;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isMeleeWeapon;
    }
}