using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    [CreateAssetMenu(menuName = "Spells/ForceSpell")]
    public class ForceSpell : SpellItem
    {
        [Header("Spell Camera Shake")]
        [SerializeField] private float cameraShakeIntensity = 2f;
        [SerializeField] private float cameraShakeTime = 0.2f;
        [SerializeField] private int spellForceAmount;
        Vector3 forceDir = Vector3.zero;

        #region ForceSpellTypes

        [Header("Spell Type")]
        public SpellType spellType;
        public enum SpellType         
        {
            Spell_Push,
            Spell_Pull,
            Spell_Bounce,
        }
        public SpellType GetSpellType() => spellType;

        #endregion
        
        public override void AttemptToCastSpell(PlayerAnimatorManager animManager,Transform targetTransform,bool inAnim)
        {
            GameObject instantWarmUpFX = Instantiate(spellWarmUpFX, animManager.transform);
            animManager.PlayTargetAnimation(spellAnimation, inAnim);
        }

        public override void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform, LayerMask getSpellMask)
        {
            GameObject instantSpellFx = Instantiate(spellCastFX, castTransform);        //VFX, particle effect
            PushSpell(playerTransform, getSpellMask);      
        }

        private void PushSpell(Transform playerTransform, LayerMask getSpellMask)
        {
            CastArea(playerTransform, getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin);   //Find target objects //IDEA: to myfunctions      

            //Multiple object can be affected.
            foreach (RaycastHit hit in hits)
            {
                forceDir = Vector3.zero;
                if (hit.collider != null)
                {
                    if (hit.point != Vector3.zero)
                    {
                        switch (spellType)
                        {
                            case SpellType.Spell_Push:
                                forceDir = (hit.point - rayOrigin).normalized;
                                break;
                            case SpellType.Spell_Pull:
                                forceDir = -(hit.point - rayOrigin).normalized;
                                break;
                            case SpellType.Spell_Bounce:
                                forceDir = Vector3.up;
                                break;
                        }
                    }

                    PlayerCameraController.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);     //CamShake

                    //IDEA: Plays vfx when spell hit or etc.

                    Rigidbody getRigid = hit.collider.GetComponent<Rigidbody>();
                    if (getRigid != null)
                    {
                        getRigid.AddForce(forceDir * spellForceAmount, ForceMode.Impulse);
                    }
                }
            }
        }
    }   
}