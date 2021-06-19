using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class ButtonController : ControllerBase, IInteractable
    {             
        #region Button Types
        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] ButtonType buttonType;
        public enum ButtonType                                //BUTTON TYPES
        {
            PressWithoutTimer,                                     // PRESS BUTTON
            PressWithTimer,                                   // PRESS BUTTON, WAIT FOR canPressTimer TIMER
            PressOnOff,                                       // SWITCH BETWEEN ON-OFF BUTTON
            PressAndHold,                                     // PRESS AND HOLD BUTTON
            PressWithoutTimerFALSE,
        }
        public ButtonType GetButtonType() => buttonType;
        #endregion
        #region Requirement Type
        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    
        RequirementTypes.RequirementType IInteractable.reqType => reqType;    // GET MY REQUIREMENT TYPE TO INTERFACE
        #endregion

        [Header("Button Settings")]                              //BUTTON VARIABLES
        [SerializeField] private Animator thisAnimator;
        [SerializeField] private int canPressTimer = 1;          //BUTTON TIMER
        [SerializeField] private bool canPress = true;                        

        private void Awake()
        {
            thisAnimator = GetComponent<Animator>();  
        }

        protected override void Start()
        {
            base.Start();
            canPress = true;

            switch (buttonType)                                 
            {
                case ButtonType.PressWithoutTimer:
                case ButtonType.PressWithTimer:
                    myCurrentStatus = true;
                    break;

                case ButtonType.PressOnOff:                      
                case ButtonType.PressAndHold:
                    myCurrentStatus = false;
                    break;
            }

            thisAnimator.SetBool("canPress", myCurrentStatus);    
        }
  
        public void Interact()
        {
            myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

            if (myControllerStatus)
            {
                switch (buttonType)
                {
                    case ButtonType.PressWithoutTimer:
                        canPressTimer = 0;
                        if (canPress)
                        {
                            myCurrentStatus = true;
                            InteractedMe(myCurrentStatus);
                        }
                        break;

                    case ButtonType.PressWithTimer:
                        if (canPress)
                        {
                            myCurrentStatus = true;
                            InteractedMe(myCurrentStatus);
                        }
                        break;

                    case ButtonType.PressOnOff:
                        myCurrentStatus = !myCurrentStatus;
                        InteractedMe(myCurrentStatus);
                        break;

                    case ButtonType.PressAndHold:
                        myCurrentStatus = true;
                        InteractedMe(myCurrentStatus);
                        break;

                    case ButtonType.PressWithoutTimerFALSE:
                        myCurrentStatus = false;
                        InteractedMe(myCurrentStatus);
                        break;
                }
            }
            else
            {
                myCurrentStatus = false;
                InteractedMe(myCurrentStatus);
            }
        }

        public override void InteractedMe(bool getBool)
        {
            StartCoroutine("WaitForTimer"); //Timer Cooldown

            if (targetID!=null)
            {
                MyGameEvents.current.SetTarget(targetID, getBool);      //Do not use myID. **important** loop problems
            }           
        }

        public override void OnSettingMe(string getID, bool getbool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);               

                if (!myControllerStatus)
                {
                    myControllerStatus = false;
                    myCurrentStatus = false;
                    InteractedMe(false);
                }
                
            }
        }

        private IEnumerator WaitForTimer()
        {
            canPress = false;
            thisAnimator.SetBool("canPress", canPress);             
            switch (buttonType)
            {
                case ButtonType.PressWithoutTimer:                        
                case ButtonType.PressWithTimer:                      
                case ButtonType.PressWithoutTimerFALSE:
                    myCurrentStatus = false;
                    yield return new WaitForSeconds(canPressTimer);  
                    canPress = true;                                  
                    thisAnimator.SetBool("canPress", canPress);      
                    break;

                case ButtonType.PressOnOff:                         
                case ButtonType.PressAndHold:                       
                    canPress = true;                                
                    thisAnimator.SetBool("canPress", myCurrentStatus);
                    break;
            }

            yield break;
        }

        public void IsStillPressing(bool getButtonStatus)
        {
            switch (buttonType)
            {
                case ButtonType.PressAndHold:
                    myCurrentStatus = getButtonStatus;
                    InteractedMe(myCurrentStatus);
                    break;
            }
        }



    }
}