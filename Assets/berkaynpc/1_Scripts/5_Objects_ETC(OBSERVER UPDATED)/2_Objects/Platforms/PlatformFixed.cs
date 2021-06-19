using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class PlatformFixed : MonoBehaviour
    {
        public Transform playerTransform;
        public CharacterController characterController;

        public bool isPlayerHere;

        Vector3 moveDirection;
        Vector3 activeGlobalPlatformPoint;
        Vector3 activeLocalPlatformPoint;
        Quaternion activeGlobalPlatformRotation;
        Quaternion activeLocalPlatformRotation;

        private void FixedUpdate()
        {
            if (isPlayerHere)
            {
                if (characterController != null)                
                {                 
                    Vector3 newGlobalPlatformPoint = transform.TransformPoint(activeLocalPlatformPoint);
                    moveDirection = newGlobalPlatformPoint - activeGlobalPlatformPoint;

                    if (moveDirection.magnitude > 0.01f)
                    {
                        characterController.Move(moveDirection);
                    }

                    Quaternion newGlobalPlatformRotation = transform.rotation * activeLocalPlatformRotation;
                    Quaternion rotDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);

                    rotDiff = Quaternion.FromToRotation(rotDiff * Vector3.up, Vector3.up) * rotDiff;
                    playerTransform.rotation = rotDiff * playerTransform.rotation;
                    playerTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);

                    UpdateMovingPlatform();
                }
            }
            else
            {
                if (moveDirection.magnitude > 0.01f)
                {
                    moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, Time.deltaTime);                   
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                characterController = other.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    isPlayerHere = true;
                    playerTransform = other.transform;
                    UpdateMovingPlatform();
                }
            }
        }
        void UpdateMovingPlatform()
        {
            activeGlobalPlatformPoint = playerTransform.position;
            activeLocalPlatformPoint = transform.InverseTransformPoint(playerTransform.position);
            //Support moving platform rotation
            activeGlobalPlatformRotation = playerTransform.rotation;
            activeLocalPlatformRotation = Quaternion.Inverse(transform.rotation) * playerTransform.rotation;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                isPlayerHere = false;
                playerTransform = null;
                characterController = null;
            }
        }
    }

    //Source: https://sharpcoderblog.com/blog/unity-3d-character-controller-moving-platform-support
}