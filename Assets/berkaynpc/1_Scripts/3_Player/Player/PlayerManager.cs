using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorManager;
        PlayerInteractor playerInteractor;

        public bool inAnim;
        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool isForced;
        public bool isPushPull;
        public bool isRolling;
        public bool isWallRunning;
        public bool isClimbing;
        public bool canDoCombo;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerInteractor = GetComponent < PlayerInteractor >();
        }

        private void Update()
        {
            inputHandler.HandleAllInputs();
            playerLocomotion.HandleAllMovements();
            inAnim = animatorManager.animator.GetBool("inAnim");
            canDoCombo = animatorManager.animator.GetBool("canDoCombo");
        }

        private void FixedUpdate()
        {
            playerLocomotion.HandleAllFixed();
        }

        private void LateUpdate()
        {
            inputHandler.a_Input = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.dPad_Up = false;
            inputHandler.dPad_Down = false;
            inputHandler.dPad_Right = false;
            inputHandler.dPad_Left = false;

            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.pushPullFlag = false;
            inputHandler.climbFlag = false;        
            
            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;      
            }
        }

    }
}