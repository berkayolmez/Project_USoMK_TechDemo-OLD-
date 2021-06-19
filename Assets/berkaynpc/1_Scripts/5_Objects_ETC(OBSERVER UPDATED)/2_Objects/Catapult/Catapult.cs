using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Catapult : ControllerBase
    {
        #region Catapult Types

        [Header("Catapult Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] CatapultTypes catapultType;
        public enum CatapultTypes                              //BUTTON TYPES
        {
            CatapultSimple,
        }
        public CatapultTypes GetCataPultType() => catapultType;

        #endregion

        [Header("Values")]
        [SerializeField] private float outForce = 1;

        [Header("Output Position")]
        [SerializeField] private Transform catapultOut;
        [SerializeField] private GameObject refObj;     //will be readonly

        public override void OnSettingMe(string getID, bool getbool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    myCurrentStatus = myControllerStatus;

                    switch (myLogicGateType)
                    {
                        case MyFunctions.LogicGateType.DontHaveGate:
                            myCurrentStatus = getbool;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !getbool;
                            break;
                    }

                    switch (catapultType)
                    {
                        case CatapultTypes.CatapultSimple:

                            if (myCurrentStatus && refObj != null)      //Protection to null problems
                            {
                                refObj.transform.position = catapultOut.position;       //Set object's position to output position
                                myFunctions.AddForceToObject(refObj, catapultOut, outForce);        //Add force to object
                            }
                            break;
                    }
                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                }
            }
        }

        /// <summary>
        ///  Check the tag of the object in field. If the tag is match, kept the object in refObj.
        ///  Targets are triggered to indicate that there is an object. (example: top light red to green)
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = other.gameObject;
                MyGameEvents.current.SetTarget(targetID, true);
            }
        }

        /// <summary>
        ///  Check the tag of the object in field. If the tag is match, clear refObj.
        ///  Targets are triggered to indicate that there is empty.(example: top light green to red)
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = null;
                MyGameEvents.current.SetTarget(targetID, false);

            }
        }  
    }
}