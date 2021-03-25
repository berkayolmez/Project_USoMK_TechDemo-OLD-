using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace project_WAST
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls playerControls;
        PlayerAttackManager attackManager;
        PlayerInventory playerInventory;

        public Vector2 movementInput;
        public float moveAmount;
        public float verticalInput;
        public float horizontalInput;
        public float mouseX;
        public float mouseY;

        public bool rb_Input;
        public bool rt_Input;
        public bool b_Input;
        public bool spaceKey;

        public bool rollFlag;
        public float rollInputTimer;
        public bool sprintFlag;   
        public bool pushPullFlag;
        public bool climbFlag;
        public bool wallRunFlag;

        public bool f_Key;
        public bool interactFlag;

        public bool intPress;
        public bool intRelease;
        public bool interact;

        private void Awake()
        {
            attackManager = GetComponentInChildren<PlayerAttackManager>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void OnEnable()
        {
            if(playerControls==null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void HandleAllInputs()
        {
            HandleIntreaction();
            HandleMovementInput();
            HandleRollInput();
            HandlePushPullInput();
            HandleAttackInput();
            //HandleWallRunInput();//askida//askida
        }

        private void HandleIntreaction()
        {
            intPress = Keyboard.current.fKey.wasPressedThisFrame;
            intRelease = Keyboard.current.fKey.wasReleasedThisFrame;

            if(intPress)
            {
                interactFlag = true;
            }
            else if(intRelease)
            {
                interactFlag = false;
            }
          
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));          
        }

        private void HandleRollInput()
        {
            b_Input = playerControls.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started; //WTF?????

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
            spaceKey = playerControls.PlayerActions.PushPull.phase == UnityEngine.InputSystem.InputActionPhase.Started; //WTF?????

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

            playerControls.PlayerActions.RB_Input.performed += ctx => rb_Input = true;
            playerControls.PlayerActions.RT_Input.performed += ctx => rt_Input = true;

            if(rb_Input)
            {
                attackManager.HandleLightAttack(playerInventory.rightWeapon);
                Debug.Log("rb basildi");
            }

            if(rt_Input)
            {
                attackManager.HandleHeavyAttack(playerInventory.rightWeapon);
                Debug.Log("rt basildi");
            }

            /*rb_Input = Keyboard.current.vKey.wasPressedThisFrame;

            if (rb_Input)
            {
                attackManager.HandleRBAction();
            }*/
        }

        private void HandleSpellInputs()
        {
          
        }

        private void HandleWallRunInput()
        {
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                wallRunFlag = true;
            }
            else
            {
                wallRunFlag=false;
            }
        } //askida//askida

    }
}