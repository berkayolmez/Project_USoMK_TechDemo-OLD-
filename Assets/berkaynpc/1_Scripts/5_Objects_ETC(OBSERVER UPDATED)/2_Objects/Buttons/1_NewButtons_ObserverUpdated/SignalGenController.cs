using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class SignalGenController : ControllerBase
    {
        [SerializeField] private float timeLength;
        public enum WaveForm { sinus, triangle, sqr, saw, inv, noise, };
        public WaveForm waveform = WaveForm.sqr;

        public float baseStart = 0.0f; // start 
        public float amplitude = 1.0f; // amplitude of the wave
        public float phase = 0.0f; // start point inside on wave cycle
        public float frequency = 0.5f; // cycle frequency per second

        protected override void Start()
        {
            base.Start();
            StartCoroutine("WaitForStart");

        }

        IEnumerator WaitForStart()
        {
            yield return new WaitForSeconds(0.1f);
            if (myControllerObjList.Count <= 0)
            {
                myControllerStatus = true;
                myCurrentStatus = true;
                StartCoroutine(WaveSignal());
            }
        }

        public override void OnSettingMe(string getID, bool getbool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
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

                    if (myCurrentStatus)
                    {
                        StartCoroutine(WaveSignal());
                    }
                    else
                    {
                        timeLength = 0;
                        MyGameEvents.current.SetSignal(targetID, timeLength,0);
                    }
                }
                else
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    StopCoroutine(WaveSignal());
                    MyGameEvents.current.SetSignal(targetID, 0, 0);
                }
            }  
        }

        IEnumerator WaveSignal()
        {
            while (true)
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
                MyGameEvents.current.SetSignal(targetID, timeLength, amplitude);

                if (myCurrentStatus)
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