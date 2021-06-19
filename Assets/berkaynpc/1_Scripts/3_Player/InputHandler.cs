using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace project_usomk
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls playerControls;
        PlayerAttackManager attackManager;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        [Header("Movement Inputs")]
        public Vector2 movementInput;
        public float moveAmount;
        public float verticalInput;
        public float horizontalInput;
        public float mouseX;
        public float mouseY;

        [Header("Buttons")]
        public bool a_Input;     //F_KEY       //INTERACT func
        public bool b_Input;     //Shift_KEY   //ROLL AND RUN funcs
        public bool rb_Input;    //H_KEY       //LIGHT ATTACK func
        public bool rt_Input;    //U_KEY       //HEAVY ATTACK func
        public bool dPad_Up;     //Switch Current spell
        public bool dPad_Down;   //Switch Current spell
        public bool dPad_Left;   //Q_KEY       //SWITCK LEFT WEAPON QUICK SLOT
        public bool dPad_Right;  //E_KEY       //SWITCK RIGHT WEAPON QUICK SLOT
        public bool spaceKey;    //Space_KEY   //PUSH AND CLIMB funcs
   
        [Header("Flags")]     //The flags are marked when the relevant buttons are pressed.
        public bool rollFlag;   
        public bool sprintFlag;   
        public bool pushPullFlag;
        public bool climbFlag;
        public bool comboFlag;

        [Header("Interactable F key vs")]
        public bool f_Key_Press;
        public bool f_Key_Release;

        private float rollInputTimer;

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                playerControls.PlayerActions.RB_Input.performed += ctx => rb_Input = true;
                playerControls.PlayerActions.RT_Input.performed += ctx => rt_Input = true;
                playerControls.PlayerQuickSlots.DpadRight.performed += ctx => dPad_Right = true;
                playerControls.PlayerQuickSlots.DpadLeft.performed += ctx => dPad_Left = true;
                playerControls.PlayerQuickSlots.DpadUp.performed += ctx => dPad_Up = true;
                playerControls.PlayerQuickSlots.DpadDown.performed += ctx => dPad_Down = true;
            }
            playerControls.Enable();
        }

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            attackManager = GetComponentInChildren<PlayerAttackManager>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        public void HandleAllInputs() //CONTROLED BY PLAYERMANAGER SC
        {
            HandleIntreaction();
            HandleMovementInput();
            HandleRollInput();
            HandlePushPullInput();
            HandleAttackInput();
            HandleQuickSlotInput();
        }

        private void HandleIntreaction()
        {
            f_Key_Press = Keyboard.current.fKey.wasPressedThisFrame;        
            a_Input = f_Key_Press;
            f_Key_Release = Keyboard.current.fKey.wasReleasedThisFrame;
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));          
        }

        private void HandleRollInput()
        {
            b_Input = playerControls.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (b_Input)
            {               
                rollInputTimer += Time.deltaTime;
                sprintFlag = true;
            }
            else
            {
                if(rollInputTimer>0 && rollInputTimer<0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandlePushPullInput()
        {
            spaceKey = playerControls.PlayerActions.PushPull.phase == UnityEngine.InputSystem.InputActionPhase.Started; 

            if (spaceKey)
            {
                pushPullFlag = true;            
            }

            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                climbFlag = true;
            }
            if(Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                climbFlag = false;
            }
        }

        private void HandleAttackInput()
        {
            if(rb_Input)
            {
                if (!playerManager.isForced && playerManager.inAnim)
                {
                    return;
                }

                attackManager.PerformRBSpellAction(playerInventory.rb_Spell);

            }

            if (rt_Input)
            {
                if (playerManager.inAnim)
                {
                    return;
                }
                attackManager.PerformRTSpellAction(playerInventory.rt_Spell);
            }
        }
        private void HandleQuickSlotInput()
        {

            if (dPad_Right)
            {
                playerInventory.SwitchToNextWeapon(false);
            }
            else if(dPad_Left)
            {
                playerInventory.SwitchToNextWeapon(true);
            }
        } 

    
        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}