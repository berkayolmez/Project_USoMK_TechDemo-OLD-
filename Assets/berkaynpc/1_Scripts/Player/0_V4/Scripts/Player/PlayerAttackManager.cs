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
        WeaponHandlerManager weaponManager;
        PlayerTargetInteractor targetInteractor;
        InputHandler inputHandler;

        [Header("Attack Things")] //ismi degistir
        public string lastAttack;
        public LayerMask spellMask;
        [SerializeField] private Transform rightHolder;

        public bool isSpellCasting = false;

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
            isSpellCasting = false;
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
            if(isSpellCasting || targetInteractor.currentTargetTransform==null) ////bunu inceleeeeee*****************************
            {
                return;
            }

            //in animdeyse cagirilmiyor o yüzden isspellcasting
            if (playerInventory.rb_Spell != null)
            {
              
                //check for FP
                playerInventory.currentSpell = playerInventory.rb_Spell;
                playerInventory.rb_Spell.AttemptToCastSpell(animatorManager,targetInteractor.currentTargetTransform,false);
                isSpellCasting = true;
                //attempt to cast spell
                StartCoroutine("WaitForSec");
            }            
        }

        public void PerformRTSpellAction(SpellItem spell)
        {
            if(isSpellCasting)
            {
                return;
            }

            if (playerInventory.rt_Spell != null)
            {
                //check for FP
                playerInventory.currentSpell = playerInventory.rt_Spell;
                playerInventory.rt_Spell.AttemptToCastSpell(animatorManager, targetInteractor.currentTargetTransform, true);
                isSpellCasting = true;
                //attempt to cast spell
                StartCoroutine("WaitForSec");
            }
        }

        private void SuccesfullyCastSpell() //animasyonda çaðýrýyoruz bunu
        {
            playerInventory.currentSpell.SuccesfullyCastSpell(rightHolder, transform, spellMask); //spell buradan tetikleniyor         
            isSpellCasting = false;

            //force buraya gelecek bir de spell fx durmuyor durdur
        }

        IEnumerator WaitForSec()
        {
            yield return new WaitForSeconds(1);
            isSpellCasting = false;
            yield break;
        }

    }
}