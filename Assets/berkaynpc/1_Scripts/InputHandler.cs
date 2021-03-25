using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace old
{
    public class InputHandler : MonoBehaviour
    {

        public InputAction Movement;
        public InputAction Rotation;
        public InputAction LastRot;

        // public PlayerControls inputActions;


        [Header("Sticks")]
        public Vector2 leftStick;
        public Vector2 rightStick;
        public Vector2 rightStickLastRot;

        [Header("D-Pad")]
        public bool dpadUp;  //pistolkey aynı zamanda
        public bool dpadDown;  //swordkey aynı zamanda
        public bool dpadRight;  // bool yerine başka bişi kullanılabilir
        public bool dpadLeft;

        [Header("Buttons")]
        public bool buttonNorth;
        public bool buttonSouth;
        public bool buttonWest;
        public bool buttonEast;

        [Header("Triggers and Shoulders")]
        public bool rightShoulder;
        public bool rightTrigger;
        public bool leftShoulder;
        public bool leftTrigger;

        [Header("Menu and others")]
        public bool menuButton;

        [Header("Keyboard")]
        public Vector2 movementInput;
        public Vector2 rotationInput;
        public bool digit1Key;
        public bool digit2Key;
        public bool qKey;
        public bool eKey;
        //buraya key gelecek
        public bool fKey;
        public bool spaceKey;
        public bool leftShift;
        public bool mouseLeft;
        public bool mouseRight;
        //buraya key gelecek
        //buraya key gelecek
        public bool menuKey;



        private void Awake()
        {

        }

        private void OnEnable()
        {
            Movement.Enable();
            Rotation.Enable();
            LastRot.Enable();


        }


        private void OnDisable()
        {
            Movement.Disable();
            Rotation.Disable();
            LastRot.Disable();


        }



        private void Start()
        {
            leftStick = new Vector2(0f, 0f);
            rightStick = new Vector2(0f, 0f);
            rightStickLastRot = new Vector2(0f, 0f);
            movementInput = new Vector2(0f, 0f);
            rotationInput = new Vector2(0f, 0f);
            dpadUp = false;
            dpadDown = false;
            dpadLeft = false;
            dpadRight = false;
            buttonNorth = false;
            buttonEast = false;
            buttonWest = false;
            buttonSouth = false;
            rightShoulder = false;
            leftShoulder = false;
            leftTrigger = false;
            rightTrigger = false;
            menuButton = false;
            digit1Key = false;
            digit2Key = false;
            qKey = false;
            eKey = false;
            fKey = false;
            spaceKey = false;
            leftShift = false;
            mouseLeft = false;
            mouseRight = false;
            menuKey = false;

        }

        private void FixedUpdate()
        {
            MovementInput();
            RotationInput();
            PCKeys();
            // GamepadButtons();
            // PressTest();

        }



        public void MovementInput()
        {
            // movementInput = inputActions.PlayerMovement.Movement.ReadValue<Vector2>();  // inputssytem eski

            movementInput = Movement.ReadValue<Vector2>();
            leftStick = movementInput;
        }

        public void RotationInput()
        {
            // rotationInput = inputActions.PlayerMovement.Rotation.ReadValue<Vector2>(); // inputssytem eski

            rotationInput = Rotation.ReadValue<Vector2>();  //aim ve ateş için
            rightStick = rotationInput;

            rightStickLastRot = LastRot.ReadValue<Vector2>();  //animasyon için

        }


        public void PCKeys()
        {
            digit1Key = Keyboard.current.digit1Key.wasPressedThisFrame;
            digit2Key = Keyboard.current.digit2Key.wasPressedThisFrame;
            qKey = Keyboard.current.qKey.wasPressedThisFrame;
            eKey = Keyboard.current.eKey.wasPressedThisFrame;
            //buraya bir key gelecek item ya da skill için
            fKey = Keyboard.current.fKey.wasPressedThisFrame;
            spaceKey = Keyboard.current.spaceKey.wasPressedThisFrame;
            leftShift = Keyboard.current.leftShiftKey.wasPressedThisFrame;
            //buraya key gelecek

            mouseLeft = Mouse.current.leftButton.wasPressedThisFrame;
            mouseRight = Mouse.current.rightButton.wasPressedThisFrame;


            menuKey = Keyboard.current.escapeKey.wasPressedThisFrame;

        }

        public void GamepadButtons()
        {

            dpadDown = Gamepad.current.dpad.down.wasPressedThisFrame;
            dpadLeft = Gamepad.current.dpad.left.wasPressedThisFrame;
            dpadRight = Gamepad.current.dpad.right.wasPressedThisFrame;
            buttonNorth = Gamepad.current.buttonNorth.wasPressedThisFrame;
            buttonWest = Gamepad.current.buttonWest.wasPressedThisFrame;
            buttonSouth = Gamepad.current.buttonSouth.wasPressedThisFrame;
            buttonEast = Gamepad.current.buttonEast.wasPressedThisFrame;
            rightShoulder = Gamepad.current.rightShoulder.wasPressedThisFrame;
            leftShoulder = Gamepad.current.leftShoulder.wasPressedThisFrame;
            rightTrigger = Gamepad.current.rightTrigger.wasPressedThisFrame;
            leftTrigger = Gamepad.current.leftTrigger.wasPressedThisFrame;
            menuButton = Gamepad.current.startButton.wasPressedThisFrame;


            //ek yapılabilir misal "Gamepad.current.rightStickButton" gibi
        }



        public void PressTest()
        {

            //Gamepad kontrol 

            if (dpadUp)
            {
                Debug.Log("dpadUp");
            }

            if (dpadDown)
            {
                Debug.Log("dpadDown");
            }

            if (dpadLeft)
            {
                Debug.Log("dpadLeft");
            }

            if (dpadRight)
            {
                Debug.Log("dpadRight");
            }

            if (buttonNorth)
            {
                Debug.Log("buttonNorth");
            }

            if (buttonEast)
            {
                Debug.Log("buttonEast");
            }

            if (buttonSouth)
            {
                Debug.Log("buttonSouth");
            }

            if (buttonWest)
            {
                Debug.Log("buttonWest");
            }

            if (rightShoulder)
            {
                Debug.Log("rightShoulder");
            }

            if (leftShoulder)
            {
                Debug.Log("leftShoulder");
            }

            if (rightTrigger)
            {
                Debug.Log("rightTrigger");
            }

            if (leftTrigger)
            {
                Debug.Log("leftTrigger");
            }

            if (menuButton)
            {
                Debug.Log("menuButton");
            }


            //Gerisi klavye kontrol

            if (digit1Key)
            {
                Debug.Log("digit1Key");
            }

            if (digit2Key)
            {
                Debug.Log("digit2Key");
            }

            if (qKey)
            {
                Debug.Log("qKey");
            }

            if (eKey)
            {
                Debug.Log("eKey");
            }

            /* if (keygelecek)
             {
                 Debug.Log("keygelecek");
             }*/

            if (fKey)
            {
                Debug.Log("fKey");
            }

            if (spaceKey)
            {
                Debug.Log("spaceKey");
            }

            if (leftShift)
            {
                Debug.Log("leftShift");
            }

            if (mouseLeft)
            {
                Debug.Log("mouseLeft");
            }

            if (mouseRight)
            {
                Debug.Log("mouseRight");
            }


            //1-2 tane daha key gelecek item ve skill için ***********

            if (menuKey)
            {
                Debug.Log("menuKey");
            }

        }



    }
}