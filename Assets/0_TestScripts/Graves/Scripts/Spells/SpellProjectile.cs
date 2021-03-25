using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class SpellProjectile : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [SerializeField] private float yOffset=0.15f;
        [SerializeField] private float sphereCastRadius=0.1f;
        [SerializeField] private float maxCastDistance = 5f;     

        public RequirementTypes.SpellElementTypes elementType;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(PlayerAnimatorManager animManager)
        {
            Debug.Log("You attemt to cast a spell!");
        }

        public virtual void SuccesfullyCastSpell(PlayerAnimatorManager animManager,Transform castTransform,Transform playerTransform,LayerMask getLayerMask)
        {
            Debug.Log("success");
        }

        public void CastArea(Transform playerTransform,LayerMask getSpellMask,out RaycastHit[] hits,out Vector3 rayOrigin)
        {
            rayOrigin = new Vector3(playerTransform.position.x, playerTransform.position.y + yOffset, playerTransform.position.z);
            hits = Physics.SphereCastAll(rayOrigin, sphereCastRadius, playerTransform.forward, maxCastDistance, getSpellMask);
        }      


    }
}