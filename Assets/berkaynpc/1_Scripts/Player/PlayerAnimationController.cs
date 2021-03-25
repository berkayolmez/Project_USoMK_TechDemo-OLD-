using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerAnimationController : AnimationManager
    {
        [SerializeField] private Animator playerAnimator;

        private void Start()
        {
            playerAnimator = GetComponent<Animator>();
        }



        public void SetAxisMove(float getVertical, float getHorizontal)
        {
            playerAnimator.SetFloat("Vertical", getVertical); 
            playerAnimator.SetFloat("Horizontal", getHorizontal);
        }
        public void SetLastAxis(float lastVertical, float lastHorizontal)
        {
            playerAnimator.SetFloat("lastMoveV", lastVertical);
            playerAnimator.SetFloat("lastMoveH", lastHorizontal);
        }

        public void SetCanMove(bool getCanMove)
        {
            playerAnimator.SetBool("canMove", getCanMove);
        }

        public void SetMoveSpeed(float getSpeed)
        {
            playerAnimator.SetFloat("moveSpeed", getSpeed);
        }

        public void SetAnimAngle(float getAnimAngle)
        {
            playerAnimator.SetFloat("angleRot", getAnimAngle, 0.0f, Time.deltaTime * 2f);
        }

        public void SetPistol(bool getPistol)
        {
            playerAnimator.SetBool("Pistol", getPistol);
        }

    }
}