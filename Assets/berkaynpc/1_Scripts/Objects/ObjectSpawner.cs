using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class ObjectSpawner : MonoBehaviour, IHaveStatus, IHaveButton
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool spawnerStatus = true;
        public bool myStatus => spawnerStatus;

        [Header("Spawner Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] SpawnerTypes spawnerType;
        public enum SpawnerTypes                                //BUTTON TYPES
        {
            SimpleSpawner,                                    // PRESS BUTTON
        }
        public SpawnerTypes GetSpawnerType() => spawnerType;

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Values")]
        [SerializeField] private float outForce=1;

        [Header("Spawn Object and Spawn Pos")]
        [SerializeField] private GameObject objectPrefab = null;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private GameObject myObj;

        [Header("Connected And Controller Objects")]       
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                spawnerStatus = controllerStatus;

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        spawnerStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        spawnerStatus = !isButtonOn;
                        break;
                }


                if(spawnerStatus)
                {
                    switch (spawnerType)
                    {
                        case SpawnerTypes.SimpleSpawner:

                            if (myObj != null)
                            {
                                StartCoroutine(DestroyObject(myObj));
                            }

                            myObj = myFunctions.SpawnObject(objectPrefab, spawnPos, outForce);  // *******(önemli) spawn pozisyonunu elle ayarla transfom.down yönünde
                            controllerStatus = false;
                            break;
                    }
                }

            }
            else if (!controllerStatus)
            {
                spawnerStatus = false;
                controllerStatus = false;
            }
        }


        IEnumerator DestroyObject(GameObject getObj) //bok gibi bir çözüm bence baþka çare bulmak lazým
        {
           
                //destroy animation gelsin
                //waitfor animation secs;
                getObj.transform.position = new Vector3(0, 250, 0);
                yield return new WaitForSeconds(0.2f);
                Destroy(getObj);
          
        }

    }
}