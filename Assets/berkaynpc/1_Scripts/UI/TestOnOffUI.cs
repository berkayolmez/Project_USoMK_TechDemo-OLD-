using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace project_WAST
{
    public class TestOnOffUI : MonoBehaviour, IHaveStatus, IHaveButton
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool uiStatus = false;
        public bool myStatus => uiStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE  

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Button Settings")]
        [SerializeField] private TextMeshProUGUI thisTextUI;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS
        
        private void Start()
        {
            thisTextUI = GetComponent<TextMeshProUGUI>();

            if (uiStatus)
            {
                thisTextUI.color = Color.green;
            }
            else
            {
                thisTextUI.color = Color.red;
            }

            PressedButton(false);
        }


        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);
           
            if (controllerStatus)
            {
                uiStatus = controllerStatus;

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        uiStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        uiStatus = !isButtonOn;
                        break;
                }

                if(uiStatus)
                {
                    thisTextUI.color = Color.green;
                }
                else
                {
                    thisTextUI.color = Color.red;
                }   
            }
            else
            {
                uiStatus = false;
                controllerStatus = false;
                thisTextUI.color = Color.red;
            }
        }
    }
}