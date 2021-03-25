using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Platforms : MonoBehaviour, IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool platformStatus = true;
        public bool myStatus => platformStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Platform Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] PlatformTypes platformType;
        public enum PlatformTypes                              //BUTTON TYPES
        {
          PlatformMove,
          PlatformMoveLoop,
          PlatformRot,
          PlatformRotLoop,
        }
        private Vector3 prevPos;
        private Vector3 transformVelocity;
        private bool isPlayerHere;
        CharacterController characterController;

        public PlatformTypes GetPlatformType() => platformType;

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Target Positions")]
        [SerializeField] private Transform startPos;
        [SerializeField] private Transform endPos;
        private Vector3 oldStartPos;
        private Vector3 oldEndPos;
        private Quaternion oldStartRot;
        private Quaternion oldEndRot;

        [Header("Values")]
        [SerializeField] private float speed=1;
        [SerializeField] private bool isStarted = false;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS


        private void Start()
        {
            if(startPos!=null && endPos!=null)
            {
                oldStartPos = startPos.position;
                oldEndPos = endPos.position;
                oldStartRot = startPos.rotation;
                oldEndRot = endPos.rotation;
            }

            if (controllerObjs.Length <= 0)
            {
                controllerStatus = true;
                platformStatus = true;
                StartCoroutine(MoveObject()); //bu deðiþecek platform türüne göre oto baþlayabilir
            }
        }

        private void FixedUpdate()
        {
            transformVelocity = (transform.position - prevPos) / Time.deltaTime;
            prevPos = transform.position;

            if (isPlayerHere)
            {
                if(characterController!=null)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }
            }
        }

        public void PressedButton(bool isButtonOn)
        {           
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                platformStatus = controllerStatus;
                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        platformStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        platformStatus = !isButtonOn;
                        break;
                }                

                if(platformStatus && !isStarted)
                {
                    StartCoroutine(MoveObject());
                }               

            }
            else if (!controllerStatus)
            {
                platformStatus = false;
                controllerStatus = false;
                StopCoroutine(MoveObject());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.CompareTag("Player"))
            {              
                characterController = other.GetComponent<CharacterController>();
                if(characterController != null)
                {                   
                    isPlayerHere = true;                  
                }
            }

            if(other.transform.CompareTag("MoveObj"))
            {
                other.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.transform.CompareTag("Player"))
            {              
                isPlayerHere = false;
            }

            if (other.transform.CompareTag("MoveObj"))
            {
                other.transform.SetParent(null);
              
            }
        } 

        IEnumerator MoveObject() //bok gibi bir çözüm bence baþka çare bulmak lazým
        {
            float elapsedTime = 0;
            bool isFin = false;
            bool getBool = false;

            switch (platformType)
            {
                case PlatformTypes.PlatformMove:
                    isStarted = true;
                    getBool = isStarted;
                    break;

                case PlatformTypes.PlatformMoveLoop:
                    getBool = platformStatus;
                    break;
            }

            while (getBool)
            {
                transform.position = Vector3.MoveTowards(startPos.position, endPos.position, (elapsedTime * speed));   //tam çalýþmýyor incele bunu  
                elapsedTime += Time.deltaTime;

                if (transform.position == endPos.position)
                {
                    if (endPos.position == oldStartPos)
                    {
                        endPos.position = oldEndPos;
                        startPos.position = transform.position;
                        isFin = true;
                        elapsedTime = 0;
                    }
                    else if (endPos.position == oldEndPos)
                    {
                        endPos.position = oldStartPos;
                        startPos.position = transform.position;
                        isFin = true;
                        elapsedTime = 0;
                    }
                }

                switch(platformType)
                {
                    case PlatformTypes.PlatformMove:                                             

                        if (isFin)
                        {
                            getBool = false;
                            isStarted = false;
                            yield break;                           
                        }
                        else
                        {
                            yield return new WaitForFixedUpdate();
                        }

                        break;

                    case PlatformTypes.PlatformMoveLoop:

                        if (platformStatus)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                        else
                        {
                            isStarted = false;
                            startPos.position = transform.position;
                            yield break;
                        }

                        break;
                }             
            }

            startPos.position = transform.position;

            yield break;
        }

        IEnumerator RotateObject() //bok gibi bir çözüm bence baþka çare bulmak lazým
        {
            float elapsedTime = 0;

            while (platformStatus)
            {
                transform.rotation = Quaternion.Lerp(startPos.rotation, endPos.rotation, (elapsedTime * speed));   //tam çalýþmýyor incele bunu  
                elapsedTime += Time.deltaTime;


                if (transform.rotation == endPos.rotation)
                {
                    if (endPos.rotation == oldStartRot)
                    {
                        endPos.rotation = oldEndRot;
                        startPos.rotation = transform.rotation;
                        elapsedTime = 0;
                    }
                    else if (endPos.rotation == oldEndRot)
                    {
                        endPos.rotation = oldStartRot;
                        startPos.rotation = transform.rotation;
                        elapsedTime = 0;
                    }
                }


                if (platformStatus)
                {
                   yield return new WaitForFixedUpdate(); 
                }
                else
                {
                    yield break;
                }
            }

            startPos.position = transform.position;

            yield break;

        }



    }
}