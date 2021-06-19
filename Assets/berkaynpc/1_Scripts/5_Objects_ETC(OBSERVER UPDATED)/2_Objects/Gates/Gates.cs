using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Gates : ObjectBase, IHold
    {
        #region Gate Types

        [Header("Gate Type")]
        [SerializeField] GateType gateType;
        public enum GateType
        {
            GateAuto,
            GateButton
        }
        public GateType GetGateType()
        {
            return gateType;
        }

        #endregion

        #region Requirement Type
        [Header("Key Type")]
        [SerializeField] private RequirementTypes.RequirementType reqType;
        RequirementTypes.RequirementType IHold.reqType => reqType;
        #endregion

        private Animator thisAnimator;
        [SerializeField] private bool isGateOpen = false;
        [SerializeField] private bool isAreaEmpty = true; //Is there a player in the trigger area or not?  

        private void Awake()
        {
            thisAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isAreaEmpty && isGateOpen && !IsAnimatorPlaying())
            {
                isGateOpen = false;
                thisAnimator.SetBool("Open", isGateOpen);
            }
        }

        public override void OnSettingMe(string getID, bool getBool) //For controller buttons
        {
            if (getID == this.myID)
            {
                switch(gateType)
                {
                    case GateType.GateButton:

                        myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                        if (myControllerStatus)
                        {
                            myCurrentStatus = getBool;
                            StartCoroutine("WaitForTimer");
                        }
                        else
                        {
                            myCurrentStatus = false;
                            StartCoroutine("WaitForTimer");
                        }
                        break;
                }               
            }                
        }

        private IEnumerator WaitForTimer()
        {
            thisAnimator.SetBool("Open", myCurrentStatus);
            yield return new WaitForSeconds(thisAnimator.GetCurrentAnimatorStateInfo(0).length); 
            StopCoroutine("WaitForTimer");
        }

        /// <summary>
        /// The player in the trigger area call this method using the interface (IHold).
        /// </summary>
        public void Holding()       //For auto gate
        {
            isAreaEmpty = false;

            if (!isGateOpen && !IsAnimatorPlaying() && !isAreaEmpty)
            {
                isGateOpen = true;
                thisAnimator.SetBool("Open", isGateOpen);
            }
        }

        /// <summary>
        /// The player leave the trigger area call this method using the interface (IHold).
        /// </summary>
        public void AreaEmpty()     //For auto gate
        {
            isAreaEmpty = true;
        }

        private bool IsAnimatorPlaying()
        {
            return thisAnimator.GetCurrentAnimatorStateInfo(0).length >
                   thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}