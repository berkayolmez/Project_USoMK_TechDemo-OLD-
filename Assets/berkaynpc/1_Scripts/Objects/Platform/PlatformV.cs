using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlatformV : MonoBehaviour
    {
        public GameObject anchor;
        public Vector3 moveDir;
        public Transform playerTransform;
        public CharacterController characterController;
        public float forceFactor = 0.1f;

        public Vector3 prevPos;
        public Vector3 transformVelo;

        private void FixedUpdate()
        {
            if (playerTransform != null)
            {
                if (anchor != null)
                {
                    moveDir = anchor.GetComponent<Rigidbody>().GetPointVelocity(anchor.transform.TransformPoint(playerTransform.localPosition));
               
                    Debug.DrawRay(anchor.transform.TransformPoint(playerTransform.localPosition),Vector3.Scale(moveDir,Vector3.right)*2, Color.blue);
                    Debug.DrawRay(anchor.transform.TransformPoint(playerTransform.localPosition), Vector3.Scale(moveDir, Vector3.forward)*2 , Color.magenta);
                   
                    Debug.DrawRay(anchor.transform.TransformPoint(playerTransform.localPosition), moveDir.normalized*2, Color.red);
                  
                    characterController.Move(moveDir * Time.deltaTime);

                    if (anchor.GetComponent<Rigidbody>().velocity.magnitude >1.5f)
                    {
                        characterController.Move((anchor.transform.position - playerTransform.position) * forceFactor * Time.deltaTime); // bunu degistir movedir normalleri vs vs vs vs
                    }
                  
                }
                else if(anchor ==null)
                {
                    transformVelo = (transform.position - prevPos);
                    prevPos = transform.position;

                    if (characterController != null)
                    {
                        characterController.Move(transformVelo);
                    }
                    
                }
            }          
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                playerTransform = other.transform;
                playerTransform.SetParent(transform);
                characterController = playerTransform.GetComponent<CharacterController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                playerTransform.SetParent(null);
                playerTransform = null;
                characterController = null;
            }
        }
    }
}