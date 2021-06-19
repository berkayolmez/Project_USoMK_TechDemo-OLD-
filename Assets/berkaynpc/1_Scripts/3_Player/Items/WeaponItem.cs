using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    //Easy way to create a new weapon type but need refactory.
    [CreateAssetMenu(menuName ="Items/Weapon")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string RightHand_Idle_01;
        public string LeftHand_Idle_01;

        //Find a better way. The animation may not be called due to a typo. //Idea: Array animationList[] 
        [Header("One Handed Attack Animations")] 
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