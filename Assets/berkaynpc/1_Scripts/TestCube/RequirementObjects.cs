using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class RequirementObjects : MonoBehaviour, IHaveButton, IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();

        [Header("My Status (True/False)")]
        [SerializeField] private bool reqObjStatus;
        public bool myStatus => reqObjStatus;

        [Header("Select a Loader Type")]
        [SerializeField] ObjectLoaderType loaderType;
        public enum ObjectLoaderType
        {
            NoLoader,
            HasLoader,
        }
        public ObjectLoaderType LoaderType => loaderType;

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Loader Values")]
        public float myLoadValue = 0;
        public float maxLoadValue;
        [SerializeField] private float loaderSpeed;
        [SerializeField] private bool isLoading;

        [Header("Connected And Controller Objects")]        
        [SerializeField] private GameObject[] connectedGameObjs;
        [SerializeField] private GameObject[] controllerObjs;
        [SerializeField] private bool controllerStatus;
        [SerializeField] private GameObject[] resetControllerObjs;
        private bool resetControllerStatus;
      
        private void Update()
        {
            if(isLoading && !reqObjStatus)
            {
                switch(loaderType)
                {
                    case ObjectLoaderType.HasLoader:
                        myLoadValue = myFunctions.Loader(loaderSpeed, maxLoadValue);
                        reqObjStatus = myFunctions.isLoadingDone;

                        if (reqObjStatus)
                        {
                            isLoading = false;
                            myLoadValue = maxLoadValue;
                            myFunctions.SetMyConnectedObjects(connectedGameObjs, true);                           
                        }
                        break;
                }              
            }
        }

        public void PressedButton(bool isButtonOn)
        {
           
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);  //bu resetin üzerinde olmalý çünkü reseten sonra düzgün çalýþmaz***

            /*resetControllerStatus = myFunctions.CheckControllerObjects(resetControllerObjs, myLogicGateType);  //reset çalýþmýyor doðru düzgün düzelt

            if (reqObjStatus && resetControllerStatus)
            {
                controllerStatus = false;
                myLoadValue = 0;
                isLoading = false;
                reqObjStatus = false;
                resetControllerStatus = false;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, reqObjStatus);
            }*/

            if (controllerStatus)
            {   
                switch (loaderType)
                {           
                    case ObjectLoaderType.NoLoader:                        
                        reqObjStatus = controllerStatus;
                        switch (myLogicGateType) //daha iyi bulunursa silinir
                        {
                             case MyFunctions.LogicGateType.DontHaveGate:
                                 reqObjStatus = isButtonOn;
                                 break;
                             case MyFunctions.LogicGateType.Not:
                                 reqObjStatus = !isButtonOn;
                                 break;
                        }
                        myFunctions.SetMyConnectedObjects(connectedGameObjs, reqObjStatus);         
                        break;

                    case ObjectLoaderType.HasLoader:        
                        if(!reqObjStatus)
                        {
                            isLoading = isButtonOn;
                        }                                        
                        break;
                }
            }       
            else if(!controllerStatus)
            {
                //reqObjStatus = false;
                isLoading = false;
                switch (loaderType)
                {
                    case ObjectLoaderType.NoLoader:                  
                        myFunctions.SetMyConnectedObjects(connectedGameObjs, false);           
                        break;

                    case ObjectLoaderType.HasLoader:                                              
                        myFunctions.SetMyConnectedObjects(connectedGameObjs, reqObjStatus);
                        break;
                }
            }
            //Dýþarýdan biþi olursa resetlemek için reset fonksiyonu yazýlabilir               
        }



    }
}