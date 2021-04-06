using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
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
                quickSlots.UpdateWeaponSlotUI(true, spellItem);     ////quick slotlarda degisiklik yapilacak onemliiiiii******

                if(spellItem != null) //handle left weapon idle animations
                {
                   
                    animatorManager.animatorOverrideController["RB_Spell"] = spellItem.spellClip;
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

                if (spellItem != null) //handle right  weapon idle animations
                {
                    animatorManager.animatorOverrideController["RT_Spell"] = spellItem.spellClip;
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