using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class WeaponHandlerManager : MonoBehaviour
    {
        PlayerAnimatorManager animatorManager;
        WeaponHandler leftHandSlot;
        WeaponHandler rightHandSlot;
        UI_QuickSlots quickSlots;
 
        private void Awake()
        {
            animatorManager = GetComponent<PlayerAnimatorManager>();
            quickSlots = FindObjectOfType<UI_QuickSlots>();

            WeaponHandler[] weaponSlots = GetComponentsInChildren<WeaponHandler>();

            foreach(WeaponHandler weaponSlot in weaponSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(SpellItem spellItem,bool isRB_Spell)
        {
            if(isRB_Spell)
            {
                leftHandSlot.LoadWeaponModel(spellItem);
                quickSlots.UpdateWeaponSlotUI(true, spellItem);

                if(spellItem != null) 
                {                   
                    animatorManager.animatorOverrideController["RB_Spell"] = spellItem.spellClip;       //Add the animation of the spell.
                }
                else
                {
                    animatorManager.animator.CrossFade("LeftArmEmpty",0.2f);
                }              
            }
            else
            {
                rightHandSlot.LoadWeaponModel(spellItem);
                quickSlots.UpdateWeaponSlotUI(false, spellItem);

                if (spellItem != null)
                {
                    animatorManager.animatorOverrideController["RT_Spell"] = spellItem.spellClip;       //Add the animation of the spell.
                }
                else
                {
                    animatorManager.animator.CrossFade("RightArmEmpty", 0.2f);
                }
            }
        }

        #region Stamina Set
        public void DrainStaminaLightAttack()
        {
            //playerRefs.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            //playerRefs.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion
    }
}