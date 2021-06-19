using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class PlayerAttackManager : MonoBehaviour
    {
        PlayerAnimatorManager animatorManager;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        WeaponHandlerManager weaponManager;
        PlayerTargetInteractor targetInteractor;
        InputHandler inputHandler;

        [Header("Attack Things")]
        public string lastAttack;
        public LayerMask spellMask;         //Target layers for spells
        [SerializeField] private Transform rightHolder;     //Right Hand

        public bool isCastingSpell = false;

        private void Awake()
        {
            playerManager=GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animatorManager = GetComponent <PlayerAnimatorManager>();
            weaponManager = GetComponent<WeaponHandlerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            targetInteractor = GetComponent<PlayerTargetInteractor>();
        }

        private void Start()
        {
            isCastingSpell = false;
        }

        #region This will be activated

        #region For combo System
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorManager.animator.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_LightAttack_1)
                {
                    animatorManager.PlayTargetAnimation(weapon.OH_LightAttack_2, true);
                }
            }
        }
        #endregion

        /*
        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponManager.attackingWeapon = weapon;
            animatorManager.PlayTargetAnimation(weapon.OH_LightAttack_1, true);
            lastAttack = weapon.OH_LightAttack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponManager.attackingWeapon = weapon;
            animatorManager.PlayTargetAnimation(weapon.OH_HeavyAttack_1, true);
            lastAttack = weapon.OH_HeavyAttack_1;
        }
        */


        #endregion

        //This method is called when the corresponding button is pressed.
        public void PerformRBSpellAction(SpellItem spell)
        {
            //If inAnim is true it is not called. That's why SpellCasting is used.
            if (isCastingSpell || targetInteractor.currentTargetTransform==null) //Check that later
            {
                return;
            }
           
            if (playerInventory.rb_Spell != null)
            {              
                playerInventory.currentSpell = playerInventory.rb_Spell;
                playerInventory.rb_Spell.AttemptToCastSpell(animatorManager,targetInteractor.currentTargetTransform,false);     //Attempt to cast spell
                isCastingSpell = true;            
                StartCoroutine("WaitForSec");
            }            
        }

        //This method is called when the corresponding button is pressed.
        public void PerformRTSpellAction(SpellItem spell)
        {
            if(isCastingSpell)
            {
                return;
            }

            if (playerInventory.rt_Spell != null)
            {
                playerInventory.currentSpell = playerInventory.rt_Spell;
                playerInventory.rt_Spell.AttemptToCastSpell(animatorManager, targetInteractor.currentTargetTransform, true);        //attempt to cast spell
                isCastingSpell = true;
                StartCoroutine("WaitForSec");
            }
        }

        private void SuccesfullyCastSpell() //This should be added as an event to the animation.
        {
            playerInventory.currentSpell.SuccesfullyCastSpell(rightHolder, transform, spellMask); //Spell is triggered from here
            isCastingSpell = false;
        }

        IEnumerator WaitForSec()
        {
            yield return new WaitForSeconds(1);
            isCastingSpell = false;
            yield break;
        }

    }
}