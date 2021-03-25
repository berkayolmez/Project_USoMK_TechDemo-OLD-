using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class OnOffGenerator : MonoBehaviour,IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();
        private MyFunctions.LogicGateType myLogicGateType = MyFunctions.LogicGateType.DontHaveGate;

        [SerializeField] private float timeLength=1;
        [SerializeField] private bool onOffStatus = false;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private bool startedBool = true;
        public bool myStatus => onOffStatus;

        public enum WaveForm { OnOff, };

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS


        private void Start()
        {
            if(controllerObjs.Length<=0)
            {
                controllerStatus = true;
                onOffStatus = true;
                StartCoroutine(StartOnOffGenerator());
            }
        }

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                onOffStatus = controllerStatus;
                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        onOffStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        onOffStatus = !isButtonOn;
                        break;
                }


                if (onOffStatus && !isStarted)
                {
                    StartCoroutine(StartOnOffGenerator());
                }
            }
            else if (!controllerStatus)
            {
                onOffStatus = false;
                controllerStatus = false;
                StartCoroutine(StartOnOffGenerator());
            }
        }

        IEnumerator StartOnOffGenerator()
        {
            isStarted = true;

            while (onOffStatus)
            {
                startedBool = !startedBool;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, startedBool);
                yield return new WaitForSeconds(timeLength);
                startedBool = !startedBool;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, startedBool);
                yield return new WaitForSeconds(timeLength);

                if (onOffStatus)
                {
                    yield return null;
                }
                else
                {
                    isStarted = false;
                    yield break;
                }
            }

            isStarted = false;
            StopCoroutine(StartOnOffGenerator());
            yield break;         
        }
    }
}