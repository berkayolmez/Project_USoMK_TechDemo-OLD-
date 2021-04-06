using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class WallRun : MonoBehaviour
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("Wall Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] WallTypes wallType;
        public enum WallTypes                              //BUTTON TYPES
        {
            WallRunSimple,
        }

        [SerializeField] private int denemeaaa;

       [SerializeField] private bool isPlayerHere;
       

        [Header("Values")]
        [SerializeField] private float speed = 1;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private Vector3 wallRunVelocity;


        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            /*wallRunVelocity = transform.forward * speed + (transform.up+transform.right)*denemeaaa;
            if (isPlayerHere)
            {
                if (characterController != null)
                {
                    characterController.Move(wallRunVelocity * Time.deltaTime);
                }
            }*/
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
              
            }
        }

        private void OnTriggerStay(Collider other)
        {
          
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
              // playerController.gravity = -9.81f;
            }
        }

    }
}