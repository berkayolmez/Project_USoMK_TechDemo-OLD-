using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace project_usomk
{
    public class Duplicator : ControllerBase
    {
        private ObjectPoolController objectPool;

        #region Duplicator Types

        [Header("Duplicator Type")]                               
        [SerializeField] DuplicatorTypes duplicatorType;
        public enum DuplicatorTypes                             
        {
            DuplicatorSimple,
        }
        public DuplicatorTypes GetDuplicatorType() => duplicatorType;

        #endregion

        [Header("Duplicator Settings")]
        [Range(1,5)]
        [SerializeField] private int maxDuplicatedObjectCount;
        [SerializeField] private float outForce=1;
        [SerializeField] private List<GameObject> duplicatedObjects;

        [Header("Output Position")]
        [SerializeField] private Transform duplicatorOut;
        [SerializeField] private GameObject refObj;     //will be readonly

        protected override void Start()
        {
            base.Start();
            objectPool = ObjectPoolController.Instance;
        }

        public override void OnSettingMe(string getID, bool getbool)
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
                            myCurrentStatus = getbool;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !getbool;
                            break;
                    }


                    switch (duplicatorType)
                    {
                        case DuplicatorTypes.DuplicatorSimple:

                            if (myCurrentStatus && refObj != null)      //Protection to null problems
                            {
                                if (duplicatedObjects.Count >= maxDuplicatedObjectCount)     //Current count of duplicated objects is compared with the maxDuplicatedObjectCount. Return the next duplicated object (duplicatedObjects[0]) to object pool if current count greater than the max count.
                                {
                                    objectPool.FindAndReturnPool(duplicatedObjects[0]);     //Find the object id then return it to pool.
                                    duplicatedObjects.Remove(duplicatedObjects[0]);         //Remove the object from list
                                }

                                GameObject newObj = objectPool.FindAndSpawn(refObj,duplicatorOut.position,duplicatorOut.rotation);      //Find the object id then spawn from pool.
                                myFunctions.AddForceToObject(newObj, duplicatorOut, outForce);      //Add a force to object
                                duplicatedObjects.Add(newObj);      //Add the object to duplicatedObjects list
                            }
                            break;
                    }

                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                }

            }
        }

        /// <summary>
        ///  Check the tag of the object in field. If the tag is match, kept the object in refObj.
        ///  Targets are triggered to indicate that there is an object. (example: top light red to green)
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = other.gameObject;
                MyGameEvents.current.SetTarget(targetID, true); //bagli lambanin rengini ayarlamak icin yazildi baska amac icin kullanilabilir
            }
        }

        /// <summary>
        ///  Check the tag of the object in field. If the tag is match, clear refObj, duplicated objects.
        ///  Targets are triggered to indicate that there is empty.(example: top light green to red)
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("MoveObj"))
            {
                refObj = null;
                foreach(var obj in duplicatedObjects)
                {
                    if (obj != null)
                    {
                        objectPool.FindAndReturnPool(obj);
                    }
                }
                duplicatedObjects.Clear();
                MyGameEvents.current.SetTarget(targetID, false);
            }        
        }


    }
}