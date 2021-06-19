using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace project_usomk
{
   // [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class Object_Movementable : MonoBehaviour, IHaveButton, IHaveStatus,IHaveSignal           ///editor yazýlacak enuma göre gösterecek*
    {
        private MyFunctions myFunctions = new MyFunctions();
        private Rigidbody myRigid;
        private IHold holding;

        [Header("My Status (True/False)")]
        [SerializeField] private bool ObjStatus;
        public bool myStatus => ObjStatus;

        [Header("Select a Loader Type")]
        public ObjectMovementType movementType;
        public enum ObjectMovementType
        {
            simpleMovementable,
            canControllable,
            ControlBySignal,
        }
        public ObjectMovementType MovementType => movementType;

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        

        [Header("Loader Values")]
        [SerializeField] private float moveSpeed=1;
        [SerializeField] private float rotSpeed=1;
        [SerializeField] private Vector3 newPos;
        [SerializeField] private Vector3 newRotate;
        public float myMovementValue_X = 0;
        public float myMovementValue_Y = 0;
        public float myMovementValue_Z = 0;

        [Header("Controller Objects")]
        [SerializeField] private GameObject[] xControllerPos;
        [SerializeField] private GameObject[] xControllerNeg;
        [SerializeField] private GameObject[] zControllerPos;
        [SerializeField] private GameObject[] zControllerNeg;
        [SerializeField] private GameObject[] yRotateControllerPos;
        [SerializeField] private GameObject[] yRotateControllerNeg;
        [SerializeField] private bool controllerStatus;
         private bool controllerStatusXpos;
         private bool controllerStatusXneg;
         private bool controllerStatusZpos;
         private bool controllerStatusZneg;
         private bool rotateControllerStatusYpos;
         private bool rotateControllerStatusYneg;

        private void Awake()
        {
            myRigid = GetComponent<Rigidbody>();

           // requirementList = new List<RequirementTypes.RequirementType>();

            //requirementList.Add(reqType); //karakterin bütün butonlarla etkileþimi iiçin bla bla bu yazýyý düzelt 
        }

        private void Update()
        {
            if (ObjStatus && controllerStatus)
            {
                ChangePosition(newPos);
                ChangeRotation(newRotate);
            }
        }

        public void PressedButton(bool isButtonOn)
        {
                switch (movementType)
                {
                    case (ObjectMovementType.canControllable): //daha güzel biþi yazýlmalý  kesin deðiþtirmem gerekiyor

                    controllerStatusXpos = myFunctions.CheckControllerObjects(xControllerPos,myLogicGateType);
                    controllerStatusXneg = myFunctions.CheckControllerObjects(xControllerNeg, myLogicGateType);
                    controllerStatusZpos = myFunctions.CheckControllerObjects(zControllerPos, myLogicGateType);
                    controllerStatusZneg = myFunctions.CheckControllerObjects(zControllerNeg, myLogicGateType);
                    rotateControllerStatusYpos = myFunctions.CheckControllerObjects(yRotateControllerPos, myLogicGateType);
                    rotateControllerStatusYneg = myFunctions.CheckControllerObjects(yRotateControllerNeg, myLogicGateType);


                    if (controllerStatusXpos || controllerStatusXneg || controllerStatusZpos || controllerStatusZneg || rotateControllerStatusYpos || rotateControllerStatusYneg)
                    {
                        controllerStatus = true;
                    }
                    else
                    {
                        controllerStatus = false;
                    }


                    if (controllerStatusXpos && xControllerPos.Length > 0)
                    {
                        newPos = new Vector3(moveSpeed, 0, 0);
                        newRotate = new Vector3(0, 0, 0);              
                    }
                    else if(controllerStatusXneg && xControllerNeg.Length > 0)
                    {
                        newPos = new Vector3(-moveSpeed, 0, 0);
                        newRotate = new Vector3(0, 0, 0);
                    }
                    else if (controllerStatusZpos && zControllerPos.Length > 0)
                    {
                        newPos = new Vector3(0, 0, moveSpeed);
                        newRotate = new Vector3(0, 0, 0);
                    }
                    else if (controllerStatusZneg && zControllerNeg.Length > 0)
                    {
                        newPos = new Vector3(0, 0, -moveSpeed);
                        newRotate = new Vector3(0, 0, 0);
                    }

                    if (rotateControllerStatusYpos)
                    {
                        newRotate = new Vector3(0, rotSpeed, 0);
                        newPos = new Vector3(0, 0, 0);
                    }
                    else if(rotateControllerStatusYneg)
                    {
                        newRotate = new Vector3(0, -rotSpeed, 0);
                        newPos = new Vector3(0, 0, 0);
                    }

                    ObjStatus = isButtonOn; //bunu kontrol et
                    break;
                }    
        }

        private void OnTriggerStay(Collider other)
        {
            holding = other.GetComponent<IHold>(); //sistemi zorlayabilir kontrol et

            if (holding != null)
            {
                if (ContainsKey(holding.reqType))
                {
                    holding.Holding();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            holding = other.GetComponent<IHold>(); //sistemi zorlayabilir kontrol et

            if (holding != null)
            {
                holding.AreaEmpty();
            }
        }

        private void ChangePosition(Vector3 getPos) //rigidbody falan daha güzel olur lerp****
        {
            myRigid.velocity = getPos * 1;            

        }

        private void ChangeRotation(Vector3 getRot)
        {
            transform.Rotate(getRot);
        }


       public bool ContainsKey(RequirementTypes.RequirementType getKeyType)
       {
            return reqType==getKeyType;
       }

        public void GetSignal(float getSignal,float maxSignal)
        {
            switch (movementType)
            {
                case ObjectMovementType.ControlBySignal:

                    newPos = new Vector3(getSignal, 0, 0);
                    newRotate = new Vector3(0, 0, 0);

                    ChangePosition(newPos);
                    ChangeRotation(newRotate);

                    break;
            }
        }
    }
}