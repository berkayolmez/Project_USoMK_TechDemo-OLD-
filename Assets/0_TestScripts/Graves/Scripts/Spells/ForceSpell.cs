using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    [CreateAssetMenu(menuName = "Spells/ForceSpell")]
    public class ForceSpell : SpellItem
    {
        [Header("Spell Camera Shake")]
        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float shakeTime = 0.2f;
        [SerializeField] private int forceAmount; //editor ile gösterme yapýlabilir bunu düzenle
        Vector3 forceDir = Vector3.zero;

        #region ForceSpellTypes

        [Header("Spell Type")]
        public SpellType spellType;
        public enum SpellType         //bunun yerine state yazýlabilir******                     
        {
            Spell_Push,
            Spell_Pull,
            Spell_Bounce,
        }
        public SpellType GetSpellType() => spellType;

        #endregion


        public override void AttemptToCastSpell(PlayerAnimatorManager animManager,bool inAnim)
        {
            GameObject instantWarmUpFX = Instantiate(spellWarmUpFX, animManager.transform);
            animManager.PlayTargetAnimation(spellAnimation, inAnim);
        }

        public override void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform, LayerMask getSpellMask)
        {
            GameObject instantSpellFx = Instantiate(spellCastFX, castTransform);
            PushSpell(playerTransform, getSpellMask);
        }

        private void PushSpell(Transform playerTransform, LayerMask getSpellMask)
        {
            CastArea(playerTransform, getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin); //myfunctionsa gidebilir bu****           

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

                    PlayerCameraController.Instance.ShakeCamera(shakeIntensity, shakeTime); //CamShake
                                                                                            //hit.pointte bir tane animasyon parlatabilirim
                    Rigidbody getRigid = hit.collider.GetComponent<Rigidbody>();
                    if (getRigid != null)
                    {
                        // Debug.Log(hit.collider.name);
                        getRigid.AddForce(forceDir * forceAmount, ForceMode.Impulse);
                    }
                }



            }
        }

    }

   
}