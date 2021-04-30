using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class SpellItem : Item
    {
        [Header("Spell Info")]
        [TextArea]
        public string spellDescription;
        public GameObject modelPrefab;
        public RequirementTypes.SpellElementTypes elementType;
        public Transform currentTarget;

        //public AnimationClip spellIdle; sonra dusunulecek
        [Header("Spell VFX")]
        public AnimationClip spellClip;
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;                         //CHOOSE A BUTTON TYPE    

        [Header("Spell Key")]
        public SpellKeys spellKey;
        public enum SpellKeys         //bunun yerine state yazýlabilir******                     
        {
            RB_Key,
            RT_Key,
        }
        public SpellKeys GetSpellKey() => spellKey;

        // [Tooltip("RB_Spell / RT_Spell bunu enuma gecirmek daha mantýklý olabilir")]
       // [HideInInspector]
        public string spellAnimation;
    
        [Header("Spell Interaction Cast Variables")]
        [SerializeField] private float yOffset=0.15f;
        [SerializeField] private float sphereCastRadius=0.1f;
        [SerializeField] private float maxCastDistance = 5f;

        public virtual void AttemptToCastSpell(PlayerAnimatorManager animManager,Transform targetTransform,bool inAnim)
        {
            Debug.Log("You attemt to cast a spell!");
        }

        public virtual void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform, LayerMask getLayerMask)
        {
            Debug.Log("success");
        }

        public void CastArea(Transform playerTransform, LayerMask getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin)
        {
            rayOrigin = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, playerTransform.position.z);
            hits = Physics.SphereCastAll(rayOrigin, sphereCastRadius, playerTransform.forward, maxCastDistance, getSpellMask);
        }


    }
}