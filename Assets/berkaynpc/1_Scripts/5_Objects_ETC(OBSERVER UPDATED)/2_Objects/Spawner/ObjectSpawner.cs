using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class ObjectSpawner : ObjectBase
    {
        private ObjectPoolController objectPool;

        #region Spawner Types

        [Header("Spawner Type")]                               //CHOOSE A Spawner TYPE     
        [SerializeField] SpawnerTypes spawnerType;
        public enum SpawnerTypes                               
        {
            SimpleSpawner,                                   
        }
        public SpawnerTypes GetSpawnerType() => spawnerType;

        #endregion

        [Header("Spawner Settings")]
        [SerializeField] private float outForce=1;      //Object spawner output power

        [Header("Spawn Object and Spawn Pos")]          
        [SerializeField] private string objectName;      //Which object will spawn from pool.
        [SerializeField] private Transform spawnPos;     //Spawn output position.
        [SerializeField] private GameObject myObj;       //Spawned object

        protected override void Start()
        {
            base.Start();
            objectPool = ObjectPoolController.Instance;
        }

        public override void OnSettingMe(string getID, bool getBool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    myCurrentStatus = myControllerStatus;

                    switch (myLogicGateType)
                    {
                        case MyFunctions.LogicGateType.DontHaveGate:
                            myCurrentStatus = getBool;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !getBool;
                            break;
                    }

                    if (myCurrentStatus)
                    {
                        switch (spawnerType)
                        {
                            case SpawnerTypes.SimpleSpawner:

                                if (myObj != null)
                                {
                                    objectPool.ReturnToPool(objectName, myObj);
                                }

                                myObj = objectPool.SpawnFromPool(objectName, spawnPos.transform.position, spawnPos.transform.rotation);      //Spawn target object from pool.
                                myFunctions.AddForceToObject(myObj, spawnPos, outForce);        //Add a force to object
                                myControllerStatus = false;
                                break;
                        }
                    }
                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                }
            }              
        }
    }
}