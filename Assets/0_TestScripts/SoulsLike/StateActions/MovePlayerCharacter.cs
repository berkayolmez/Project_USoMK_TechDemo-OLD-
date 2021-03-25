using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    public class MovePlayerCharacter : StateAction
    {
        PlayerStateManager states;


        public bool isGrounded;

        public MovePlayerCharacter(PlayerStateManager playerStateManager)
        {
            states = playerStateManager;
        }


        public override bool Execute()
        {
            float frontY = 0;
            RaycastHit hit;
            Vector3 origin = states.mTransform.position + (states.mTransform.forward * states.frontRayOffset);
            origin.y += 0.5f;
            Debug.DrawRay(origin, -Vector3.up, Color.red, .01f, false);
            isGrounded = Physics.Raycast(origin, -Vector3.up, out hit, 1, states.groundLayer);
            states.isGrounded = isGrounded;
            if (isGrounded) //151 ignore 10.22 ben deðiþtirdim ground layer yaptým
            {
                float y = hit.point.y;
                frontY = y - states.mTransform.position.y;
            }

            Vector3 currentVelocity = states.rigidbody.velocity;
            //Vector3 targetVelocity = states.mTransform.forward * states.moveAmount * states.movementSpeed;
            Vector3 targetVelocity = new Vector3(states.horizontal, 0, states.vertical) * states.moveAmount * states.movementSpeed;            
            
            if (states.isGrounded)
            {
                float moveAmount = states.moveAmount;

                if(moveAmount >0.1f)
                {
                    states.rigidbody.isKinematic = false;
                    states.rigidbody.drag = 0;

                    if(Mathf.Abs(frontY)>0.02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * states.movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);

                    if(abs>0.02f)
                    {
                        states.rigidbody.isKinematic = false;
                        targetVelocity.y = 0;
                        states.rigidbody.drag = 4;
                    }
                }

                HandleRotation();
            }
            else
            {
                states.rigidbody.isKinematic = false;
                states.rigidbody.drag = 0;
                targetVelocity.y = currentVelocity.y;
            }

         

            Debug.DrawRay((states.mTransform.position + Vector3.up * 0.2f), targetVelocity, Color.green, 0.01f, false);
            states.rigidbody.velocity = Vector3.Lerp(currentVelocity, targetVelocity, states.delta * states.adaptSpeed);
            //states.rigidbody.velocity = targetVelocity;  

            return false;
        }

        void HandleRotation()
        {
            Vector3 targetDirection = Vector3.zero;

            targetDirection = new Vector3(states.horizontal, 0f, states.vertical);
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
            {
                targetDirection = states.mTransform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(states.mTransform.rotation, targetRotation, states.rotSpeed * Time.deltaTime);

            states.mTransform.rotation = playerRotation;
        }
    }
}