using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class CircuitClock : MonoBehaviour,IHaveStatus,IHaveButton
    {
        private MyFunctions myFunctions = new MyFunctions();
        private MyFunctions.LogicGateType myLogicGateType=MyFunctions.LogicGateType.DontHaveGate;

        [SerializeField] private float timeLength;
        [SerializeField] private bool clockStatus = false;
        public enum WaveForm { sinus, triangle, sqr, saw, inv, noise,};
        public WaveForm waveform = WaveForm.sqr;       

        public float baseStart = 0.0f; // start 
        public float amplitude = 1.0f; // amplitude of the wave
        public float phase = 0.0f; // start point inside on wave cycle
        public float frequency = 0.5f; // cycle frequency per second

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS
        public bool myStatus => clockStatus;

        private void Start()
        {            
            if(controllerObjs.Length<=0)
            {
                controllerStatus = true;
                clockStatus = true;
                StartCoroutine(WaveSignal());
            }
        }

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                clockStatus = controllerStatus; 

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        clockStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        clockStatus = !isButtonOn;
                        break;
                }            

                if (clockStatus)
                {                   
                    StartCoroutine(WaveSignal());
                }
                else
                {                 
                    timeLength = 0;
                    myFunctions.SetMyConnectedObjects(connectedGameObjs, timeLength, 0);
                }              
            }
            else
            {
                clockStatus = false;
                controllerStatus = false;
                StopCoroutine(WaveSignal());
                myFunctions.SetMyConnectedObjects(connectedGameObjs, 0,0);
            }

        }


        IEnumerator WaveSignal()
        {
            while(true)
            {
                float y = 1.0f;
                float x = (Time.time + phase) * frequency;
                x = x - Mathf.Floor(x); // normalized value (0..1)          

                    switch (waveform)
                    {

                        case WaveForm.sinus:
                            y = Mathf.Sin(x * 2 * Mathf.PI);
                            break;

                        case WaveForm.triangle:
                            if (x < 0.5f)
                                y = 4.0f * x - 1.0f;
                            else
                                y = -4.0f * x + 3.0f;
                            break;

                        case WaveForm.sqr:
                            if (x < 0.5f)
                                y = 1.0f;
                            else
                                y = -1.0f;
                            break;

                        case WaveForm.saw:
                            y = x;
                            break;

                        case WaveForm.inv:
                            y = 1.0f - x;
                            break;

                        case WaveForm.noise:
                            y = 1f - (Random.value * 2);
                            break;
                    }              

                timeLength = (y * amplitude) + baseStart;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, timeLength, amplitude);

                if(clockStatus)
                {
                    yield return null;
                }
                else
                {
                    yield break;
                }       
            }
        }
    }
}