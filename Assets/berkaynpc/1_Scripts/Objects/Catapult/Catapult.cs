using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Catapult : MonoBehaviour,IHaveStatus,IHaveButton
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool catapultStatus = true;
        public bool myStatus => catapultStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] CatapultTypes catapultType;
        public enum CatapultTypes                              //BUTTON TYPES
        {
            CatapultSimple,
        }
        public CatapultTypes GetCataPultType() => catapultType;

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Values")]
        [SerializeField] private float outForce = 1;

        [Header("Target Positions")]
        [SerializeField] private Transform catapultOut;
        [SerializeField] private GameObject refObj;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS



        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                catapultStatus = controllerStatus;

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        catapultStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        catapultStatus = !isButtonOn;
                        break;
                }


                switch (catapultType)
                {
                    case CatapultTypes.CatapultSimple:

                        if (catapultStatus && refObj != null)
                        {
                            refObj.transform.position = catapultOut.position;
                            myFunctions.AddForceToObjects(refObj, catapultOut, outForce);
                        }
                        break;
                }
            }
            else if (!controllerStatus)
            {
                catapultStatus = false;
                controllerStatus = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = other.gameObject;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other);
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = null;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, false);

            }
        }

        IEnumerator DestroyObject(GameObject getObj) //bok gibi bir çözüm bence baþka çare bulmak lazým
        {
            //destroy animation gelsin
            //waitfor animation secs;
            getObj.transform.position = new Vector3(0, 250, 0);
            yield return new WaitForSeconds(0.2f);
            Destroy(getObj);
            StopCoroutine("DestroyObject");

        }

    }
}