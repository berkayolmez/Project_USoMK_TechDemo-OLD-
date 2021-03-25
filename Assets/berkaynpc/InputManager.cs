using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace project_WAST
{
    public class InputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        public Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;


        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void AllInputs()
        {
            MovementInput();
        }

        private void MovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
        }

        private void RotationInput()
        {

        }

    }
}