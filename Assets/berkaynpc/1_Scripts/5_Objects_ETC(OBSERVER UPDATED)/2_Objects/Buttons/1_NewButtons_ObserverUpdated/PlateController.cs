using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class PlateController : ControllerBase, IHold
    {
        #region PlateTypes
        //PLATE TYPES
        [Header("Button Type")]                     //Choose a pressure plate type      
        [SerializeField] PlateType pressurePlateType;
        public enum PlateType                      //Pressure plate types
        {
            StepOn,
            StayOn,
            StepAndHold,
            StayAndHold,
        }
        public PlateType PPlateType => pressurePlateType;
        #endregion
        #region Requirement Types
        [Header("Requirement Type")]
        [SerializeField] private RequirementTypes.RequirementType reqType;               //
        RequirementTypes.RequirementType IHold.reqType => reqType;        //Interface reading key type  
        #endregion

        [Header("Plate Settings")]
        [SerializeField] private string resetID; //ID used to reset pressure plate. Its not necessary. Just an option
        [SerializeField] private bool isAreaEmpty = true;  //Is there a player or cube on the pressure plate or not?

        [Header("For Loader Values")]
        public float loadingValue = 0;
        [SerializeField] private float maxLoadingValue = 1;
        public float timerSpeed = 1;
        [SerializeField] private int onObjectCount; //Number of those on the pressure plate. //It can be a problem when the cube and the player are on it at the same time.

        protected override void Start()
        {
            base.Start();    
        }

        private void Update()
        {
            /*
            if(targetID.Contains(myID)) //Protection for id problems  //that will be changed (ID pool solution)
            {
                Debug.LogWarning("myID = targetID");
                myID = myID + "(1)";
            }*/

            switch (pressurePlateType)
            {
                case PlateType.StepAndHold:
                    if (!isAreaEmpty && !myCurrentStatus && loadingValue <= 1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);

                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            myCurrentStatus = true;
                            loadingValue = 1;
                            InteractedMe(myCurrentStatus);
                        }
                    }
                    break;

                case PlateType.StayAndHold:

                    if (!isAreaEmpty && !myCurrentStatus && loadingValue <= 1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);
                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            myCurrentStatus = true;
                            loadingValue = 1;
                            InteractedMe(myCurrentStatus);
                        }
                    }
                    else if (isAreaEmpty && !myCurrentStatus && loadingValue > 0)
                    {
                        loadingValue = myFunctions.Loader(-timerSpeed * 0.01f, maxLoadingValue);

                        if (loadingValue <= 0)
                        {
                            loadingValue = 0;
                        }
                    }
                    break;

                case PlateType.StayOn:
                    if (isAreaEmpty)
                    {
                        myCurrentStatus = false;
                        loadingValue = 0;
                        ResetValues();
                    }
                    break;
            }
        }

        /// <summary>
        /// The player or cubes on the pressure plate call this method using the interface (IHold).
        /// </summary>
        public void Holding()
        {
            myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

            onObjectCount++;

            if (myControllerStatus && onObjectCount <= 1)
            {
                isAreaEmpty = false;

                switch (pressurePlateType)
                {
                    case PlateType.StepOn:
                        myCurrentStatus = !myCurrentStatus;

                        if (myCurrentStatus)
                        {
                            loadingValue = 1;
                        }
                        else
                        {
                            loadingValue = 0;
                        }
                        break;

                    case PlateType.StayOn:
                        loadingValue = 1;
                        myCurrentStatus = true;
                        break;
                }

                InteractedMe(myCurrentStatus);
            }
            else if (!myControllerStatus)
            {
                myCurrentStatus = false;
                InteractedMe(myCurrentStatus);
            }
        }

        /// <summary>
        /// The player leave on the plate call this method using the interface (IHold).
        /// </summary>
        public void AreaEmpty()
        {
            onObjectCount--;

            if (onObjectCount <= 0)
            {
                isAreaEmpty = true;
            }
        }

        public override void InteractedMe(bool getBool)
        {
            if (targetID != null)
            {
                MyGameEvents.current.SetTarget(targetID, getBool); //Do not use myID. **important** loop problems
            }
        }

        public override void OnSettingMe(string getID, bool getbool) //For reset
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);
                if (!myControllerStatus)
                {
                    ResetValues();
                }
            }

            if(getID==this.resetID)
            {
                ResetValues();
            }
        }


        public void ResetValues()
        {
            myCurrentStatus = false;
            loadingValue = 0;
            myFunctions.loadingValue = 0;
            myControllerStatus = false;
            MyGameEvents.current.SetTarget(targetID, false);
        }


    }
}