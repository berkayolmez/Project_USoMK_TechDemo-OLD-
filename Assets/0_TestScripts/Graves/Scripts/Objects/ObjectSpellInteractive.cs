using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace project_WAST
{
    public class ObjectSpellInteractive : MonoBehaviour, ISpellInteractive
    {
        [Header("Requirement Element Type")] 
        [SerializeField] private RequirementTypes.SpellElementTypes myElementType; 

        public void SpellInteract(RequirementTypes.SpellElementTypes getSpellElement)
        {
            if(ContainsElement(getSpellElement))
            {
                Debug.Log("spelInteract içindeyiz");
            }
        }

        public bool ContainsElement(RequirementTypes.SpellElementTypes getSpellElement)
        {
            return myElementType == getSpellElement;
        }
    }
}