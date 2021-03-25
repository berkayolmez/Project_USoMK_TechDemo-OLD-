using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void HandleAllMovement()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {/*
            float frontY = 0;
            RaycastHit hit;
            Vector3 origin = transform.position + (transform.forward * frontRayOffset);
            origin.y += 0.5f;
            Debug.DrawRay(origin, -Vector3.up, Color.red, .01f, false);
            isGrounded = Physics.Raycast(origin, -Vector3.up, out hit, 1, groundLayer);
            isGrounded = isGrounded;
            if (isGrounded) //151 ignore 10.22 ben deðiþtirdim ground layer yaptým
            {
                float y = hit.point.y;
                frontY = y - transform.position.y;
            }

            Vector3 currentVelocity = rb.velocity;
            //Vector3 targetVelocity = states.mTransform.forward * states.moveAmount * states.movementSpeed;
            Vector3 targetVelocity = new Vector3(horizontal, 0, vertical) * moveAmount * movementSpeed;

            if (isGrounded)
            {
                float moveAmount = moveAmount;

                if (moveAmount > 0.1f)
                {
                    rb.isKinematic = false;
                    rb.drag = 0;

                    if (Mathf.Abs(frontY) > 0.02f)
                    {
                        targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * movementSpeed;
                    }
                }
                else
                {
                    float abs = Mathf.Abs(frontY);

                    if (abs > 0.02f)
                    {
                        rb.isKinematic = false;
                        targetVelocity.y = 0;
                        rb.drag = 4;
                    }
                }

                HandleRotation();
            }
            else
            {
                rb.isKinematic = false;
                rb.drag = 0;
                targetVelocity.y = currentVelocity.y;
            }



            Debug.DrawRay((transform.position + Vector3.up * 0.2f), targetVelocity, Color.green, 0.01f, false);
            rb.velocity = Vector3.Lerp(currentVelocity, targetVelocity, delta * adaptSpeed);
            //states.rb.velocity = targetVelocity;  */
        }

        private void HandleRotation()
        {

        }
    }
}