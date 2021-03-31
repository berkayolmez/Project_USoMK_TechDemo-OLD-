using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    [CreateAssetMenu(menuName = "Spells/InteractionSpell")]
    public class InteractionSpell : SpellItem
    {
        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float shakeTime = 0.2f;

        #region InteractionSpellTypes

        [Header("Spell Type")]
        public InteractiveSpells intSpell;
        public enum InteractiveSpells         //bunun yerine state yazılabilir******                     
        {
            Spell_SimpleInteraction
        }
        public InteractiveSpells GetIntSpells() => intSpell;

        #endregion

        public override void AttemptToCastSpell(PlayerAnimatorManager animManager,bool inAnim)
        {
            switch (spellKey)
            {
                case SpellKeys.RB_Key:
                    spellAnimation = "RB_Spell";
                    break;

                case SpellKeys.RT_Key:
                    spellAnimation = "RT_Spell";
                    break;
            }

            if (spellWarmUpFX!=null)
            {
                GameObject instantWarmUpFX = Instantiate(spellWarmUpFX, animManager.transform);
            }
            animManager.PlayTargetAnimation(spellAnimation, inAnim);
        }

        public override void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform, LayerMask getSpellMask)
        {
            if(spellCastFX!=null)
            {
                GameObject instantSpellFx = Instantiate(spellCastFX, castTransform);
            }
            InteractToObj(playerTransform, getSpellMask);
        }

        private void InteractToObj(Transform playerTransform, LayerMask getSpellMask)
        {    
            switch (spellKey)
            {
                case SpellKeys.RB_Key:
                    PlayerTargetInteractor targetInteractor = playerTransform.GetComponentInChildren<PlayerTargetInteractor>();
                    if(targetInteractor!=null)
                    {
                        ISpellInteractive spellInteractive = targetInteractor.currentTargetTransform.GetComponent<ISpellInteractive>();

                        if(spellInteractive!=null)
                        {
                            spellInteractive.SpellInteract(elementType);
                            PlayerCameraController.Instance.ShakeCamera(shakeIntensity, shakeTime);
                        }

                    }
                    break;

                case SpellKeys.RT_Key:
                    Debug.Log("Rt KEY içindeyiz");
                    /*
                    CastArea(playerTransform, getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin);
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            ISpellInteractive getInteractable = hit.collider.GetComponent<ISpellInteractive>();
                            if (getInteractable != null)
                            {
                                PlayerCameraController.Instance.ShakeCamera(shakeIntensity, shakeTime);
                                getInteractable.SpellInteract(elementType); //spelltype göre etkileþime girebilir
                            }
                        }
                    }*/

                    break;
            }

            
        }

    }
}