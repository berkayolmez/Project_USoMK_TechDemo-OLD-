using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class LightBridge : ObjectBase
    {
        #region Bridge Types

        [Header("Bridge Type")]                              
        [SerializeField] BridgeTypes bridgeType;
        public enum BridgeTypes                             
        {
            BridgeSimple,
        }

        public BridgeTypes GetBridgeType() => bridgeType;

        #endregion

        [Header("Bridge Settings")]
        [SerializeField] private float speed = 1;
        [SerializeField] private bool canOpen = true;
        [SerializeField] private Vector3 scaleFactor = new Vector3(1f,1f,1f);
        [SerializeField] private bool isStarted = false;
        private Vector3 startedScale;

        protected override void Start()
        {
            base.Start();
            startedScale = transform.localScale;
            canOpen = true;
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
                        StartCoroutine(ChangeBridgeStatus());
                    }

                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    StopCoroutine(ChangeBridgeStatus());
                }

            }
        }

        IEnumerator ChangeBridgeStatus()
        {
            isStarted = true;
            
            while (canOpen)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, scaleFactor, speed);       //Open the bridge until it reaches the scaleFactor.

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
                transform.localScale = Vector3.MoveTowards(transform.localScale, startedScale, speed);      //Close the bridge until it reaches the startedScale.

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