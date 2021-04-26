using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Portals : MonoBehaviour,IHaveStatus,IHaveButton
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool platformStatus = true;
        public bool myStatus => platformStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] PortalTypes portalType;
        public enum PortalTypes                              //BUTTON TYPES
        {
            PortalSimple,
            PortalTwoWay,
            PortalAuto,
        }
        public PortalTypes GetPortalType() => portalType;
     
        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Values")]
        [SerializeField] private float portalOutForce=1;

        [Header("Target Positions")]
        [SerializeField] private GameObject portalGate;
        [SerializeField] private GameObject inObjs;
        private Vector3 oldStartPos;
        private Vector3 oldEndPos;
        private Quaternion oldStartRot;
        private Quaternion oldEndRot;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS
        [SerializeField] private Transform otherPortal;


        private void Start()
        {
           /* BoxCollider portalGateCollider= portalGate.AddComponent<BoxCollider>();
            portalGateCollider.isTrigger = true;*/

        }

        public void PressedButton(bool isButtonOn)
        {
            if(inObjs!=null && isButtonOn)
            {
                if (inObjs.CompareTag("Player"))
                {
                    CharacterController cc = inObjs.GetComponent<CharacterController>();
                    PlayerInteractor playerInteractor = inObjs.GetComponent<PlayerInteractor>();
                    cc.enabled = false;
                    inObjs.transform.position = otherPortal.position;
                    cc.enabled = true;
                    inObjs = null;

                }
                else if (inObjs.CompareTag("MoveObj")) //daha güzel yöntem bulunabilir*********
                {
                    inObjs.transform.position = otherPortal.position;
                    myFunctions.AddForceToObjects(inObjs, otherPortal, portalOutForce);
                    inObjs = null;
                }
            }              
           
        }

        private void OnTriggerEnter(Collider other)
        {
            inObjs = other.gameObject;
            switch(portalType)
            {
                case PortalTypes.PortalAuto:

                   if (inObjs != null)
                    {
                        if (inObjs.CompareTag("Player"))
                        {
                            CharacterController cc = inObjs.GetComponent<CharacterController>();
                            cc.enabled = false;
                            inObjs.transform.position = otherPortal.position;
                            cc.enabled = true;
                            inObjs = null;
                        }
                        else if (inObjs.CompareTag("MoveObj")) //daha güzel yöntem bulunabilir*********
                        {
                            inObjs.transform.position = otherPortal.position;
                            myFunctions.AddForceToObjects(inObjs, otherPortal, portalOutForce);
                            inObjs.transform.SetParent(null);
                            inObjs = null;
                        }
                    }

                    break;
            }
            //inObjs.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inObjs = null;
                // inObjs.Remove(other.gameObject);
            }

            if(other.CompareTag("MoveObj"))
            {
                inObjs = null;
            }
        }

        private void TeleportObj()
        {
           
        }


    }
}