using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Teleporter : ObjectBase
    {
        #region Teleporter Types

        [Header("Teleporter Type")]                               //CHOOSE A Teleporter TYPE     
        [SerializeField] TeleporterTypes teleporterType;
        public enum TeleporterTypes                              
        {
            TeleporterSimple,
            TeleporterTwoWay,
            TeleporterAuto,
        }
        public TeleporterTypes GetPortalType() => teleporterType;
        #endregion              

        [Header("Teleporter Settings")]
        [SerializeField] private float portalOutForce=1;

        [Header("Target Positions")]
        [SerializeField] private GameObject inObjs;
        [SerializeField] private Transform outputPortal;

        protected override void Start()
        {
            base.Start();
        }

        public override void OnSettingMe(string getID, bool getBool)
        {
            if (getID == this.myID)
            {
                if (inObjs != null && getBool)
                {
                    if (inObjs.CompareTag("Player"))
                    {
                        CharacterController cc = inObjs.GetComponent<CharacterController>();
                        PlayerInteractor playerInteractor = inObjs.GetComponent<PlayerInteractor>();
                        cc.enabled = false;
                        inObjs.transform.position = outputPortal.position;
                        cc.enabled = true;
                        inObjs = null;

                    }
                    else if (inObjs.CompareTag("MoveObj"))
                    {
                        inObjs.transform.position = outputPortal.position;
                        myFunctions.AddForceToObject(inObjs, outputPortal, portalOutForce);
                        inObjs = null;
                    }
                }
            }               
        }

        private void OnTriggerEnter(Collider other)
        {
            inObjs = other.gameObject;
            switch(teleporterType)
            {
                case TeleporterTypes.TeleporterAuto:

                    if (inObjs != null)
                    {
                        if (inObjs.CompareTag("Player"))
                        {
                            CharacterController cc = inObjs.GetComponent<CharacterController>();        //Character controller will be disabled for teleport. It's necessary**
                            cc.enabled = false;
                            inObjs.transform.position = outputPortal.position;
                            cc.enabled = true;
                            inObjs = null;
                        }
                        else if (inObjs.CompareTag("MoveObj"))
                        {
                            inObjs.transform.position = outputPortal.position;
                            inObjs.transform.rotation = outputPortal.rotation;
                            myFunctions.AddForceToObject(inObjs, outputPortal, portalOutForce);
                            inObjs.transform.SetParent(null);
                            inObjs = null;
                        }
                    }
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                inObjs = null;
            }

            if(other.CompareTag("MoveObj"))
            {
                inObjs = null;
            }
        }
    }
}