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
        FieldOfView playerFov;
        WeaponHandlerManager weaponManager;
        InputHandler inputHandler;

        [Header("Attack Things")] //ismi degistir
        public string lastAttack;
        public LayerMask spellMask;
        [SerializeField] private Transform rightHolder;

        public bool canUseRB = true;

        private void Awake()
        {
            playerManager=GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animatorManager = GetComponent <PlayerAnimatorManager>();
            playerFov = GetComponent<FieldOfView>();
            weaponManager = GetComponent<WeaponHandlerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        private void Start()
        {
            canUseRB = true;
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.comboFlag)
            {
                animatorManager.animator.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_LightAttack_1)
                {
                    animatorManager.PlayTargetAnimation(weapon.OH_LightAttack_2, true);
                }
            }
        }
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

        public void PerformRBSpellAction(SpellItem spell)
        {
            if (playerInventory.rb_Spell != null)
            {
                //check for FP
                playerInventory.currentSpell = playerInventory.rb_Spell;
                playerInventory.rb_Spell.AttemptToCastSpell(animatorManager,false);
                //attempt to cast spell
            }            
        }

        public void PerformRTSpellAction(SpellItem spell)
        {
            if (playerInventory.rt_Spell != null)
            {
                //check for FP
                playerInventory.currentSpell = playerInventory.rt_Spell;
                playerInventory.rt_Spell.AttemptToCastSpell(animatorManager,true);

                //attempt to cast spell
            }
        }

        private void SuccesfullyCastSpell() //animasyonda çaðýrýyoruz bunu
        {
            playerInventory.currentSpell.SuccesfullyCastSpell(rightHolder,transform, spellMask); //spell buradan tetikleniyor          
            //force buraya gelecek bir de spell fx durmuyor durdur
        }

    }
}