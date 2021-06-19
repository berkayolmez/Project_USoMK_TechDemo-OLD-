using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class UI_KeyController : MonoBehaviour
    {
        public PlayerInteractor interactor;

        private void Awake()
        {
            interactor = GetComponentInParent<PlayerInteractor>();
        }
        public void RemoveKeys()
        {
            interactor.DeleteKey(RequirementTypes.RequirementType.RedKey);
            interactor.DeleteKey(RequirementTypes.RequirementType.GreenKey);
            interactor.DeleteKey(RequirementTypes.RequirementType.BlueKey);
        }

        public void AddRedKey()
        {
            interactor.GetKey(RequirementTypes.RequirementType.RedKey);
        }

        public void AddGreenKey()
        {
            interactor.GetKey(RequirementTypes.RequirementType.GreenKey);
        }

        public void AddBlueKey()
        {
            interactor.GetKey(RequirementTypes.RequirementType.BlueKey);
        }
    }
}