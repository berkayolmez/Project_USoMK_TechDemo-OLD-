using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class LaserDetector : MonoBehaviour, IReflectable, IHaveStatus
    {
        //OTHER SCRIPTS AND INTERFACES
        private MyFunctions myFunctions = new MyFunctions();

        [Header("My Status (True/False)")] // THIS BUTTON'S STATUS
        [SerializeField] private bool laserDetectorStatus = true;
        public bool myStatus => laserDetectorStatus;

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.LaserReqTypes laserReqType;    //
        RequirementTypes.LaserReqTypes IReflectable.laserReqType => laserReqType;    // GET MY REQUIREMENT TYPE TO INTERFACE

        [Header("Connected Objects And Butons")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON

        public void Reflect(bool getBool, float getLaserLength)
        {
            //Debug.Log("aaaa " + this.gameObject.name);
            myFunctions.SetMyConnectedObjects(connectedGameObjs, getBool);
        }

    }
}