using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class AreaTrigger : MonoBehaviour , IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool areaStatus = true;
        public bool myStatus => areaStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] AreaTypes areaType;
        public enum AreaTypes                                //BUTTON TYPES
        {
            AreaSimple,   
        }
        public AreaTypes GetAreaType() => areaType;      

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS

        private void Start()
        {
            if(controllerObjs.Length<=0)
            {
                controllerStatus = true;
            }
        }

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, MyFunctions.LogicGateType.DontHaveGate);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                if (controllerStatus)
                {
                    areaStatus = true;
                    myFunctions.SetMyConnectedObjects(connectedGameObjs, true);
                }
                else
                {
                    areaStatus = false;
                }
            }           
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                areaStatus = false;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, false);
            }
        }

    }
}