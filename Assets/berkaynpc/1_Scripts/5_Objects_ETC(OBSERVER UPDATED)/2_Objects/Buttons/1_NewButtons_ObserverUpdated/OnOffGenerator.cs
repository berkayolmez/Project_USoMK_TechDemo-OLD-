using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class OnOffGenerator : ControllerBase
    {
        [Header("Generator Settings")]
        [SerializeField] private float timeLength = 1;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private bool startedBool = true;
        public enum WaveForm { OnOff, };

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
                StartCoroutine(StartOnOffGenerator());
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

                    if (myCurrentStatus && !isStarted)
                    {
                        StartCoroutine(StartOnOffGenerator());
                    }
                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    StartCoroutine(StartOnOffGenerator());
                }
            }
        }

        IEnumerator StartOnOffGenerator()
        {
            isStarted = true;

            while (myCurrentStatus)
            {
                startedBool = !startedBool;
                MyGameEvents.current.SetTarget(targetID, startedBool);      //Set On to target objects
                yield return new WaitForSeconds(timeLength);
                startedBool = !startedBool;
                MyGameEvents.current.SetTarget(targetID, startedBool);      //Set Off to target objects
                yield return new WaitForSeconds(timeLength);

                if (myCurrentStatus)
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

