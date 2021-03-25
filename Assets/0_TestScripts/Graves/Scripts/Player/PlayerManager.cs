using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        PlayerInteractor playerInteractor;
        Animator anim;

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
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerInteractor = GetComponent < PlayerInteractor >();
        }

        private void Update()
        {
            inputHandler.HandleAllInputs();
            inAnim = anim.GetBool("inAnim");
            playerLocomotion.HandleAllMovement();
        }

        private void FixedUpdate()
        {
            playerLocomotion.HandleAllFixed();
            playerInteractor.HandleFixedInteractors();
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
            inputHandler.pushPullFlag = false;
            inputHandler.climbFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.wallRunFlag = false;
            
            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;      
            }
        }
    }
}