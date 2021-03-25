using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace project_WAST
{
    public class Duplicator : MonoBehaviour,IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool duplicatorStatus = true;
        public bool myStatus => duplicatorStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Button Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] DuplicatorTypes duplicatorType;
        public enum DuplicatorTypes                              //BUTTON TYPES
        {
            DuplicatorSimple,
        }
        public DuplicatorTypes GetDuplicatorType() => duplicatorType;       

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Values")]
        [Range(1,5)]
        [SerializeField] private int duplicateLength;
        [SerializeField] private float outForce=1;
        [SerializeField] private List<GameObject> duplicatedObjects;

        [Header("Target Positions")]
        [SerializeField] private Transform duplicatorOut;
        [SerializeField] private GameObject refObj;
        private Vector3 oldStartPos;
        private Vector3 oldEndPos;
        private Quaternion oldStartRot;
        private Quaternion oldEndRot;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] connectedGameObjs;
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS
    


        public void PressedButton(bool isButtonOn)
        {           
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                duplicatorStatus = controllerStatus;

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        duplicatorStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        duplicatorStatus = !isButtonOn;
                        break;
                }


                switch (duplicatorType)
                {
                    case DuplicatorTypes.DuplicatorSimple:
                   
                        if (duplicatorStatus  && refObj != null)
                        {

                            if(duplicatedObjects.Count >= duplicateLength)
                            {
                                StartCoroutine(DestroyObject(duplicatedObjects[0]));
                                duplicatedObjects.Remove(duplicatedObjects[0]);
                            }

                            GameObject newObj = myFunctions.SpawnObject(refObj, duplicatorOut, outForce);                          
                            duplicatedObjects.Add(newObj);
                        }
                        break;
                }

            }
            else if (!controllerStatus)
            {
                duplicatorStatus = false;
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
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = null;
                foreach(var obj in duplicatedObjects)
                {
                    if (obj != null)
                    {
                        StartCoroutine(DestroyObject(obj));
                    }
                }
                duplicatedObjects.Clear();
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