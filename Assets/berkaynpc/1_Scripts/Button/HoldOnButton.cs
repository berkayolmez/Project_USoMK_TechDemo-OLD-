using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace project_WAST
{
    public class HoldOnButton : MonoBehaviour, IHold , IHaveButton, IHaveStatus
    {
        //OTHER SCRIPTS AND INTERFACES
        private MyFunctions myFunctions = new MyFunctions();

        [Header("My Status (True/False)")] // THIS BUTTON'S STATUS
        [SerializeField] private bool holdButtonStatus = true;
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

        [Header("UI Objects")]  //UI for demo 
        [SerializeField] private TextMeshProUGUI loadingText;  
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Image circle;

        [Header("Values")]
        [SerializeField]  private float loadingValue = 0;
        [SerializeField] private float maxLoadingValue = 1;
        [SerializeField]  private float timerSpeed = 1;
        [SerializeField] private int onObjectCount; //butona basanlarýn toplam sayýsý

        [Header("Connected Objects And Butons")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;   // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;                   // CHECK ALL CONNECTED BUTTONS
        [SerializeField] private GameObject[] resetController; //silinebilir kontrol et bunu

        private void Awake() //Starting settings
        {
            loadingText.text = "LOADING: %0";
            timerSlider.value = timerSpeed;  
        }

        private void Update()
        {
            switch (buttonType)
            {
                case ButtonType.StepAndHold:
                    if (!isAreaEmpty && !holdButtonStatus && loadingValue <= 1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);
                        circle.fillAmount = loadingValue;
                        loadingText.text = "LOADING: %" + (loadingValue * 100).ToString("F0");
                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            holdButtonStatus = true;
                            loadingValue = 1;
                            PressedMe(holdButtonStatus);
                            //CmdDo();
                        }
                    }
                    break;

                case ButtonType.StayAndHold: 

                    if(!isAreaEmpty && !holdButtonStatus && loadingValue<=1)
                    {
                        loadingValue = myFunctions.Loader(timerSpeed * 0.01f, maxLoadingValue);
                        circle.fillAmount = loadingValue;
                        loadingText.text = "LOADING: %" + (loadingValue * 100).ToString("F0");
                        if (loadingValue >= 1)
                        {
                            myFunctions.loadingValue = 0;
                            holdButtonStatus = true;
                            loadingValue = 1;
                            PressedMe(holdButtonStatus);
                            //CmdDo();
                        }
                    }
                    else if (isAreaEmpty && !holdButtonStatus && loadingValue > 0)
                    {
                        loadingValue = myFunctions.Loader(-timerSpeed * 0.01f, maxLoadingValue);
                        circle.fillAmount = loadingValue;
                        loadingText.text = "LOADING: %" + (loadingValue * 100).ToString("F0");

                        if (loadingValue <= 0)
                        {
                            loadingValue = 0;
                        }
                    }
                    break;

                case ButtonType.StayOn:
                    if (isAreaEmpty && circle.fillAmount > 0)
                    {
                        holdButtonStatus = false;
                        circle.fillAmount = 0;
                        loadingText.text = "LOADING: %" + (loadingValue * 100).ToString("F0");
                        ResetValues();
                    }
                    break;
            }
        }

        /*
        [Command(ignoreAuthority = true)]
        private void CmdDo()
        {
            RpcDo();
        }

        [ClientRpc]
        private void RpcDo()
        {  
            myFunctions.SetMyConnectedObjects(connectedGameObjs, true);   
        }
        */

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
                            circle.fillAmount = 1;
                        }
                        else
                        {
                            circle.fillAmount = 0;
                        }
                        break;

                    case ButtonType.StayOn:
                      circle.fillAmount = 1;
                        holdButtonStatus = true;                       
                    //  CmdDo();                       
                        break;

                    case ButtonType.StepAndHold:
                    case ButtonType.StayAndHold:
                                     
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


        public void SetTimerSpeed()
        {
            timerSpeed = timerSlider.value;
            timerText.text = "TIMER: " + timerSpeed.ToString("F0");
        }

        public void ResetValues()
        {
            holdButtonStatus = false;
            loadingValue = 0;
            myFunctions.loadingValue = 0;
            circle.fillAmount = 0;
            loadingText.text = "LOADING: %" + (loadingValue * 100).ToString("F0");

            controllerStatus = false; //reset ui        
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