using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    [CreateAssetMenu(menuName = "Spells/InteractionSpell")]
    public class InteractionSpell : SpellProjectile
    {
        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float shakeTime = 0.2f;

        #region InteractionSpellTypes

        [Header("Spell Type")]
        public InteractiveSpells intSpell;
        public enum InteractiveSpells         //bunun yerine state yazýlabilir******                     
        {
            Spell_SimpleInteraction
        }
        public InteractiveSpells GetIntSpells() => intSpell;

        #endregion

        public override void AttemptToCastSpell(PlayerAnimatorManager animManager)
        {
            GameObject instantWarmUpFX = Instantiate(spellWarmUpFX, animManager.transform);
            animManager.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccesfullyCastSpell(PlayerAnimatorManager animManager, Transform castTransform, Transform playerTransform, LayerMask getSpellMask)
        {
            GameObject instantSpellFx = Instantiate(spellCastFX, castTransform);
            animManager.PlayTargetAnimation(spellAnimation, true);

            InteractToObj(playerTransform, getSpellMask);
        }

        private void InteractToObj(Transform playerTransform, LayerMask getSpellMask)
        {
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
            }
        }

    }
}