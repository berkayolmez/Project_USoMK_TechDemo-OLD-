using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerAttackManager : MonoBehaviour
    {
        PlayerAnimatorManager animatorManager;
        PlayerInventory playerInventory;
        PlayerManager playerManager;        

        public LayerMask spellMask;
        [SerializeField] private Transform rightHolder;

        private void Awake()
        {
            playerManager=GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animatorManager = GetComponent <PlayerAnimatorManager>();
            //weaponManager = GetComponent<WeaponHandlerManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {

        }
        public void HandleLightAttack(WeaponItem weapon)
        {
           animatorManager.PlayTargetAnimation(weapon.OH_LightAttack_1, true);
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorManager.PlayTargetAnimation(weapon.OH_HeavyAttack_1, true);
        }

        //input actions
        public void HandleRBAction()
        {
            if (animatorManager.animator.GetBool("inAnim"))
            {
                return;
            }

            if(playerInventory.rightWeapon.isMeleeWeapon)
             {
                 //handle Melee
                 PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster) //bunun yerine tek bir bool gelebilir******
            {
                //magic action
                PerformRBSpellAction(playerInventory.rightWeapon);
            }

        }
      
        // attac actions>
        private void PerformRBMeleeAction()
        {

            if (playerManager.canDoCombo)
            {
                //comboFlag = true;
                // HandleWeaponCombo(playerInventory.rightWeapon);
                //comboFlag = false;
            }
            else
            {
                if (playerManager.inAnim || playerManager.canDoCombo)
                {
                    return;
                }

                //animatorManager.animator.SetBool("isUsingRightHand", true);
                //HandleLightAttack(playerInventory.rightWeapon);

            }
        }

        private void PerformRBSpellAction(WeaponItem weapon)
        {
            if(weapon.isSpellCaster)
            {
                if(playerInventory.currentSpell!=null)
                {
                    //check for FP
                    playerInventory.currentSpell.AttemptToCastSpell(animatorManager);
                    //attempt to cast spell
                }
            }
        }

        private void SuccesfullyCastSpell() //animasyonda çaðýrýyoruz bunu
        {
            //playerInventory.currentSpell.SuccesfullyCastSpell(animatorManager, rightHolder,transform, spellMask); //spell buradan tetikleniyor          
            //force buraya gelecek bir de spell fx durmuyor durdur
        }

    }
}