using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
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

        public override void AttemptToCastSpell(PlayerAnimatorManager animManager,Transform targetTransform,bool inAnim)
        {
            currentTarget = targetTransform;
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
                GameObject instantWarmUpFX = Instantiate(spellWarmUpFX, animManager.transform);     //VFX, particle effect
            }
            animManager.PlayTargetAnimation(spellAnimation, inAnim);
        }

        public override void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform,LayerMask getSpellMask)
        {
            if(spellCastFX!=null)
            {
                GameObject instantSpellFx = Instantiate(spellCastFX, castTransform);
            }
            InteractToObj(playerTransform,currentTarget, getSpellMask);          
        }

        private void InteractToObj(Transform playerTransform,Transform targetTransform, LayerMask getSpellMask)
        {    
            switch (spellKey)
            {
                //Single object can be affected.
                case SpellKeys.RB_Key:
                   
                    if(targetTransform!=null)
                    {
                        ISpellInteractive spellInteractive = targetTransform.GetComponent<ISpellInteractive>();

                        if (spellInteractive != null)
                        {
                            spellInteractive.SpellInteract(elementType);        //Triggers to the target spell interactive object.
                            PlayerCameraController.Instance.ShakeCamera(shakeIntensity, shakeTime);     //CamShake
                        }
                    }

                    break;

                case SpellKeys.RT_Key: //need more spell variation
                    /*
                    CastArea(playerTransform, getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin); //Find target objects //IDEA: to myfunctions      
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            ISpellInteractive getInteractable = hit.collider.GetComponent<ISpellInteractive>();
                            if (getInteractable != null)
                            {
                                PlayerCameraController.Instance.ShakeCamera(shakeIntensity, shakeTime);
                                getInteractable.SpellInteract(elementType);
                            }
                        }
                    }
                    */

                    break;
            }

            currentTarget = null;
        }


    }
}