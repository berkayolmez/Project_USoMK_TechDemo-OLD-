using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class AreaTriggerController : ControllerBase
    {
        #region Area Type
        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] AreaTypes areaType;
        public enum AreaTypes                                //BUTTON TYPES
        {
            AreaSimple,
        }
        public AreaTypes GetAreaType() => areaType;
        #endregion
        #region Requirement Types
        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    
        #endregion

        protected override void Start()
        {
            base.Start();
        }

        private void OnTriggerEnter(Collider other)
        {           
            if (other.gameObject.CompareTag("Player"))
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    myCurrentStatus = true;
                    MyGameEvents.current.SetTarget(targetID, myCurrentStatus);
                }
                else
                {
                    myCurrentStatus = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);
                if(myControllerStatus)
                {
                    myCurrentStatus = false;
                    MyGameEvents.current.SetTarget(targetID, myCurrentStatus);
                }
            }
        }
    }
}