using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class RepairableObjects : ControllerBase
    {
        #region Repairable Object Types

        [Header("Select a Repairable Object Type")]
        [SerializeField] ObjectLoaderType loaderType;
        public enum ObjectLoaderType
        {
            NoLoader,
            HasLoader,
        }
        public ObjectLoaderType LoaderType => loaderType;

        #endregion

        [Header("Repairable Object Settings")]
        public float myLoadValue = 0;
        public float maxLoadValue;
        [SerializeField] private float loaderSpeed;
        [SerializeField] private bool isLoading;
      
        private void Update()
        {
            if(isLoading && !myCurrentStatus)
            {
                switch(loaderType)
                {
                    case ObjectLoaderType.HasLoader:
                        myLoadValue = myFunctions.Loader(loaderSpeed, maxLoadValue);
                        myCurrentStatus = myFunctions.isLoadingDone;

                        if (myCurrentStatus)
                        {
                            isLoading = false;
                            myLoadValue = maxLoadValue;
                            MyGameEvents.current.SetTarget(targetID, true);
                        }
                        break;
                }              
            }
        }

        public override void OnSettingMe(string getID, bool getbool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);
               
                if (myControllerStatus)
                {
                    switch (loaderType)
                    {
                        case ObjectLoaderType.NoLoader:
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
                            MyGameEvents.current.SetTarget(targetID, myCurrentStatus);
                            break;

                        case ObjectLoaderType.HasLoader:
                            if (!myCurrentStatus)
                            {
                                isLoading = getbool;
                            }
                            break;
                    }
                }
                else if (!myControllerStatus)
                {
                    isLoading = false;
                    switch (loaderType)
                    {
                        case ObjectLoaderType.NoLoader:
                            MyGameEvents.current.SetTarget(targetID, false);
                            break;

                        case ObjectLoaderType.HasLoader:
                            MyGameEvents.current.SetTarget(targetID, myCurrentStatus);
                            break;
                    }
                }    
            }
        }
    }
}