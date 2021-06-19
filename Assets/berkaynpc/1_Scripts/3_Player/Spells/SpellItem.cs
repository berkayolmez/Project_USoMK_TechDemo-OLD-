using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class SpellItem : Item
    {
        [Header("Spell Info")]
        [TextArea]
        public string spellDescription;
        public GameObject modelPrefab;
        public RequirementTypes.SpellElementTypes elementType;
        public Transform currentTarget;

        //public AnimationClip spellIdle; //think about that

        [Header("Spell VFX")]
        public AnimationClip spellClip;
        public GameObject spellWarmUpFX;    //after press key
        public GameObject spellCastFX;      //after succes          

        [Header("Spell Key")]  //spell gamepad or keyboard key
        public SpellKeys spellKey;
        public enum SpellKeys                         
        {
            RB_Key,
            RT_Key,
        }
        public SpellKeys GetSpellKey() => spellKey;

        //[HideInInspector]
        public string spellAnimation;
    
        [Header("Spell Interaction Cast Variables")]
        [SerializeField] private float yOffset=0.15f;       //Spell's origin points y offset
        [SerializeField] private float sphereCastRadius=0.1f;       //The radius of the area that spells can affect.
        [SerializeField] private float maxCastDistance = 5f;        //Max distance between player and spell interactive object.

        /// <summary>
        /// Prepare to casting spell.
        /// Spell instantiate warmup vfx etc
        /// </summary>
        public virtual void AttemptToCastSpell(PlayerAnimatorManager animManager,Transform targetTransform,bool inAnim)
        {
            Debug.Log("You attemt to cast a spell!");
        }

        /// <summary>
        /// Spells are performed from here
        /// The target spell type will be triggered by this method. (PushSpell,InteractToObj ... etc.)
        /// </summary>
        public virtual void SuccesfullyCastSpell(Transform castTransform, Transform playerTransform, LayerMask getLayerMask)
        {
            Debug.Log("success");
        }

        /// <summary>
        /// This method calculates the area that the spell can affect. Objects inside the area are detected as target objects.
        /// </summary>
        public void CastArea(Transform playerTransform, LayerMask getSpellMask, out RaycastHit[] hits, out Vector3 rayOrigin)
        {
            rayOrigin = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, playerTransform.position.z);
            hits = Physics.SphereCastAll(rayOrigin, sphereCastRadius, playerTransform.forward, maxCastDistance, getSpellMask);

            //write wall detection.
        }


    }
}