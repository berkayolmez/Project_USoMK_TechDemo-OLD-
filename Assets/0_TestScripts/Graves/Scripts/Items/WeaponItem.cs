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

        [Header("Idle Animations")]
        public string RightHand_Idle_01;
        public string LeftHand_Idle_01;

        [Header("One Handed Attack Animations")] //array gelebilir*** birde string yerine baska yol bulunabilir cünkü harf hatasindan animasyon cagirilmayabilir*****
        public string OH_LightAttack_1;
        public string OH_LightAttack_2;
        public string OH_HeavyAttack_1;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isMeleeWeapon;

        [Header("Stamina Cost")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}