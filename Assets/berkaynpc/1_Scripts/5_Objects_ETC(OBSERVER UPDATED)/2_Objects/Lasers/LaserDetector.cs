using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class LaserDetector : ControllerBase, IReflectable
    {
        [Header("Requirement Type")]                                             //Detectable laser type
        [SerializeField] private RequirementTypes.LaserReqTypes laserReqType;    
        RequirementTypes.LaserReqTypes IReflectable.laserReqType => laserReqType;    // GET MY TYPE TO INTERFACE
        public void Reflect(bool getBool, float getLaserLength)
        {
            myCurrentStatus = getBool;
            MyGameEvents.current.SetTarget(targetID, myCurrentStatus);      //Set targets to myCurrentStatus
        }

    }
}