using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Object_Pushable : MonoBehaviour,IPushable
    {
        public PushCubes pushCubeType;
        private MyFunctions myFunctions = new MyFunctions();
        private Rigidbody myRigidbody;
        private IHold holding;

        [Header("My Status (True/False)")]
        [SerializeField] private bool ObjStatus;
        public bool myStatus => ObjStatus;

        [Header("Select a Loader Type")]
        [SerializeField] ObjectPushType pushType;
        public enum ObjectPushType
        {
            simpleMovementable,
            canControllable,
        }
        public ObjectPushType PushType => pushType;
        public Vector3 myCurrentVelocity => objVelocity;

        private Vector3 objVelocity;

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
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
            holding = other.GetComponent<IHold>();
            if (holding != null)
            {
                holding.AreaEmpty();
            }
        }

        public bool ContainsKey(RequirementTypes.RequirementType getKeyType)
        {
            return reqType == getKeyType;
        }

        public void Pushing(Vector3 pushDir)
        {
            myRigidbody.velocity = pushDir;
            objVelocity = myRigidbody.velocity;
           // Debug.Log("targetvelo "+ objVelocity);
        }

        public void NotPushing()
        {
            myRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}