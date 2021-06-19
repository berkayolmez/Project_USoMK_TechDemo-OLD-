using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Object_Pushable : MonoBehaviour,IPushable //IDEA: scriptable object, base class etc.
    {
        private Rigidbody myRigidbody;
        private IHold holding;

        #region Push Object Type

        [Header("Select a Push Type")]
        [SerializeField] ObjectPushType pushType;
        public enum ObjectPushType
        {
            simpleMovementable,
        }
        public ObjectPushType PushType => pushType;

        #endregion
        #region Requirement Type
        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.RequirementType reqType;    //    
        #endregion
        #region Object Velocity
        public Vector3 myCurrentVelocity => objVelocity;  
        private Vector3 objVelocity;
        #endregion

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            holding = other.GetComponent<IHold>();

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
        }

        public void NotPushing()
        {
            myRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}