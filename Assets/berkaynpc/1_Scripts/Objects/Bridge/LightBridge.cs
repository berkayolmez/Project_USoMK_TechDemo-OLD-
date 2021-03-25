using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class LightBridge : MonoBehaviour, IHaveButton, IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool BridgeStatus = false;
        public bool myStatus => BridgeStatus;

        [Header("Bridge Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] BridgeTypes bridgeType;
        public enum BridgeTypes                              //BUTTON TYPES
        {
            BridgeSimple,
        }

        public BridgeTypes GetBridgeType() => bridgeType;

        [Header("Bridge Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [SerializeField] private float speed = 1;
        [SerializeField] private Vector3 scaleFactor = new Vector3(1f,1f,1f);
        [SerializeField] private bool isStarted = false;
        private Vector3 startedScale;
        [SerializeField] private bool canOpen = true;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS

        private void Start()
        {
            startedScale = transform.localScale;
            canOpen = true;
        }

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                BridgeStatus = controllerStatus;
                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        BridgeStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        BridgeStatus = !isButtonOn;
                        break;
                }

                if (BridgeStatus && !isStarted)
                {
                    StartCoroutine(ChangeBridgeStatus());
                }

            }
            else if (!controllerStatus)
            {
                BridgeStatus = false;
                controllerStatus = false;
                StopCoroutine(ChangeBridgeStatus());
            }
        }

        IEnumerator ChangeBridgeStatus()
        {
            isStarted = true;
            
            while (canOpen)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, scaleFactor, speed);

                if (transform.localScale == scaleFactor)
                {
                    canOpen = false;
                    isStarted = false;
                    yield break;
                }
                yield return null;
            }

            while (!canOpen)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, startedScale, speed);               

                if (transform.localScale == startedScale)
                {
                    canOpen = true;
                    isStarted = false;
                    yield break;
                }
                yield return null;
            }

            isStarted = false;
            yield return null;
        }



    }
}