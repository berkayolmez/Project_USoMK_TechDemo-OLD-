using System.Collections;
using UnityEngine;

namespace project_WAST
{
    public class PressButton : MonoBehaviour, IInteractable , IHaveStatus ,IHaveButton
    {                                                              //OTHER SCRIPTS AND INTERFACES      
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********
                                                             
        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool pressButtonStatus = true; 
        public bool myStatus => pressButtonStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        #region Button Types
        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] ButtonType buttonType;
        public enum ButtonType                                //BUTTON TYPES
        {
            PressNoTimer,                                     // PRESS BUTTON
            PressWithTimer,                                   // PRESS BUTTON, WAIT FOR canPressTimer TIMER
            PressOnOff,                                       // SWITCH BETWEEN ON-OFF BUTTON
            PressAndHold,                                     // PRESS AND HOLD BUTTON
            PressWithoutTimerFALSE,
        }
        public ButtonType GetButtonType() => buttonType;
        #endregion
        #region Requirement Type
        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //
        RequirementTypes.RequirementType IInteractable.reqType => reqType;    // GET MY REQUIREMENT TYPE TO INTERFACE
        #endregion

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Button Settings")]                              //BUTTON VARIABLES
        [SerializeField] private Animator thisAnimator;       
        [SerializeField] private int canPressTimer = 1;          // THIS BUTTON'S PRESS TIMER
        [SerializeField] private bool canPress = true;           // CHECK BUTTON'S PRESSABLE      

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;  // ALL CONNECTED OBJECTS TO THIS BUTTON
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS

        private void Awake()
        {         
            thisAnimator = GetComponent<Animator>();           
        }

        private void Start()
        {
            canPress = true; 
            
            switch (buttonType)                                  //OBJECT INSTANTIATE SETTINGS
            {
                case ButtonType.PressOnOff:                      //THIS TWO TYPE COULD BE OFF WHEN START
                case ButtonType.PressAndHold:

                    pressButtonStatus = false;
                    break;
            }

            thisAnimator.SetBool("canPress", pressButtonStatus);           //SET THIS BUTTON'S START ANIMATION STATE
        }
         
        public void Interact()  // Player pressed to F //butonla ilgili sýkýntý var bazen basýyor bazen basmýyor sebebi belli deðil
        { 
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);
          
            if (controllerStatus)
            {
                switch (buttonType)
                {
                    case ButtonType.PressNoTimer:
                        canPressTimer = 0;
                        if (canPress)
                        {
                            pressButtonStatus = true;
                            PressedMe(pressButtonStatus);
                        }
                        break;

                    case ButtonType.PressWithTimer:
                        if (canPress)
                        {
                            pressButtonStatus = true;
                            PressedMe(pressButtonStatus);
                        }
                        break;

                    case ButtonType.PressOnOff:
                        pressButtonStatus = !pressButtonStatus;
                        PressedMe(pressButtonStatus);
                        break;

                    case ButtonType.PressAndHold:
                        pressButtonStatus = true;
                        PressedMe(pressButtonStatus);
                        break;

                    case ButtonType.PressWithoutTimerFALSE:
                        pressButtonStatus = false;
                        PressedMe(pressButtonStatus);
                        break;


                }              
            }
            else
            {
                pressButtonStatus = false;
                PressedMe(pressButtonStatus); 
            }
        }      

        private void PressedMe(bool getBool)                   //IF PRESS to THIS BUTTON SET ALL connectedGameObj's STATUS  //bu rpc ye gidebilir mi ???????? 
        {
            StartCoroutine("WaitForTimer");
            myFunctions.SetMyConnectedObjects(connectedGameObjs, getBool);             
        }

        public void StillPress(bool buttonStatus)               // CHECK PLAYER STILL HOLDING F KEY
        {
            switch (buttonType)
            {
                case ButtonType.PressAndHold:
                    pressButtonStatus = buttonStatus;
                    PressedMe(pressButtonStatus);
                    break;
            }
        }

        public void PressedButton(bool isButtonOn) //controller objects send this
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);
            if (!controllerStatus)
            {
                controllerStatus = false;
                pressButtonStatus = false;
                PressedMe(false);
            }
        }

        private IEnumerator WaitForTimer()                          //WAIT FOR canPressTimer
        {
            canPress = false;
            thisAnimator.SetBool("canPress", canPress);             //yeni baðlanan biri kapý açýksa kapalý görüyor *** bunu düzelt ******************************
            switch (buttonType)
            {
                case ButtonType.PressNoTimer:                        // PRESS BUTTON TYPE WITHOUT TIMER (canPressTimer = 0)
                case ButtonType.PressWithTimer:                      // PRESS BUTTON TYPE WITH TIMER (canPressTimer = SET IN EDITOR) 
                case ButtonType.PressWithoutTimerFALSE:
                    pressButtonStatus = false;
                    yield return new WaitForSeconds(canPressTimer);  // WAIT FOR canPressTimer SECS
                    canPress = true;                                 // PLAYERS CAN PRESS BUTTON      
                    thisAnimator.SetBool("canPress", canPress);      // SET ANIMATION STATE
                    break;

                case ButtonType.PressOnOff:                          // ON-OFF BUTTON TYPE
                case ButtonType.PressAndHold:                        // HOLD BUTTON TYPE
                    canPress = true;                                 // Players can press button 
                    thisAnimator.SetBool("canPress", pressButtonStatus);    // Set animation
                    break;
            }

            yield break;   //Stop coroutine
        }


    }
}

