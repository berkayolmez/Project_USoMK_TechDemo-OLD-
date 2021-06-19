using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace project_usomk
{
    public class HoldOnButton : MonoBehaviour, IHold , IHaveButton, IHaveStatus
    {
        //OTHER SCRIPTS AND INTERFACES
        private MyFunctions myFunctions = new MyFunctions();

        [Header("My Status (True/False)")] // THIS BUTTON'S STATUS
        public bool holdButtonStatus = true;
        public bool myStatus => holdButtonStatus;
        [SerializeField] private bool isAreaEmpty = true;
        private bool resetAreaStatus=false;

        //PLATE TYPES
        [Header("Button Type")]                     //Choose a pressure plate type      
        [SerializeField] ButtonType buttonType; 
        public enum ButtonType                      //Pressure plate types
        {
            StepOn,
            StayOn,
            StepAndHold,
            StayAndHold,
        }
        public ButtonType HoldType => buttonType;

        [Header("Requirement Type")]      
        [SerializeField] private RequirementTypes.RequirementType reqType;               //
        RequirementTypes.RequirementType IHold.reqType => reqType;        //Interface reading key type  

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;       

        [Header("Values")]
        public float loadingValue = 0;
        [SerializeField] private float maxLoadingValue = 1;
        public float timerSpeed = 1;
        [SerializeField] private int onObjectCount; //butona basanlarýn toplam sayýsý

        [Header("Connected Objects And Butons")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;   // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;                   // CHECK ALL CONNECTED BUTTONS
        [SerializeField] private GameObject[] resetController; //silinebilir kontrol et bunu
   

        private void Update()
        {
            switch (buttonType)
            {
                case ButtonType.StepAndHold:
                    if (!isAreaEmpty && !holdButtonStatus && loadingValue <= 1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);
                     
                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            holdButtonStatus = true;
                            loadingValue = 1;
                            PressedMe(holdButtonStatus);
                        }
                    }
                    break;

                case ButtonType.StayAndHold: 

                    if(!isAreaEmpty && !holdButtonStatus && loadingValue<=1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);
                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            holdButtonStatus = true;
                            loadingValue = 1;
                            PressedMe(holdButtonStatus);
                        }
                    }
                    else if (isAreaEmpty && !holdButtonStatus && loadingValue > 0)
                    {
                        loadingValue = myFunctions.Loader(-timerSpeed * 0.01f, maxLoadingValue);

                        if (loadingValue <= 0)
                        {
                            loadingValue = 0;
                        }
                    }
                    break;

                case ButtonType.StayOn:
                    if (isAreaEmpty)
                    {
                        holdButtonStatus = false;
                        loadingValue = 0;
                        ResetValues();
                    }
                    break;
            }
        }

        public void Holding()
        {
           controllerStatus = myFunctions.CheckControllerObjects(controllerObjs,myLogicGateType);

           onObjectCount++;

            if (controllerStatus && onObjectCount<=1)
            {
                isAreaEmpty = false;               

                switch (buttonType)
                {
                    case ButtonType.StepOn:
                        holdButtonStatus = !holdButtonStatus;

                        if(holdButtonStatus)
                        {
                            loadingValue = 1;
                        }
                        else
                        {
                            loadingValue = 0;
                        }
                        break;

                    case ButtonType.StayOn:
                        loadingValue = 1;
                        holdButtonStatus = true;           
                        break;             
                }
              
                PressedMe(holdButtonStatus);
            } 
            else if(!controllerStatus)
            {
                holdButtonStatus = false;
                PressedMe(holdButtonStatus);
            }
        }

      
        public void AreaEmpty()
        {
            onObjectCount--;

            if (onObjectCount<=0)
            {
                isAreaEmpty = true;                
            }
        }
        
        private void PressedMe(bool getBool)
        {
            myFunctions.SetMyConnectedObjects(connectedGameObjs, getBool);
        }

        public void ResetValues()
        {
            holdButtonStatus = false;
            loadingValue = 0;
            myFunctions.loadingValue = 0;
            controllerStatus = false;     
            myFunctions.SetMyConnectedObjects(connectedGameObjs, false);
        }

        public void PressedButton(bool isButtonOn) //resete gerek olmayabilir belkiii ???
        {
            resetAreaStatus = myFunctions.CheckControllerObjects(resetController, myLogicGateType);  
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);   
            if (!controllerStatus || (resetAreaStatus && !isButtonOn))
            {
                ResetValues();
                controllerStatus = false;
                holdButtonStatus = false;
            }                 
        }

    }
}