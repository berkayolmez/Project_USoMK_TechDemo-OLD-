using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class HackPanel_UI : MonoBehaviour,IInteractable
    {
        public RequirementTypes.RequirementType reqType => throw new System.NotImplementedException();

        public void Interact()
        {

        }

        public void StillPress(bool buttonStatus)
        {
            throw new System.NotImplementedException();
        }
    }
}