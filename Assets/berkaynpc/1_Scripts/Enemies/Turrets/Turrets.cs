using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Turrets : MonoBehaviour, IHaveButton, IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();

        TurretLocomotion turretLocomotion;
        FieldOfView fieldOfView;
        public Animator anim;

        [Header("My Status (True/False)")]
        [SerializeField] private bool turretStatus=true;
        public bool myStatus => turretStatus;

        public bool isRotDefault = true;
        public bool isAreaEmpty=true;
        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [SerializeField] private GameObject[] connectedGameObjs;
        [SerializeField] private GameObject[] controllerObjs;
        [SerializeField] private bool controllerStatus;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            turretLocomotion = GetComponent<TurretLocomotion>();
            fieldOfView = GetComponent<FieldOfView>();
        }

        private void Start()
        {
            if(controllerObjs.Length<=0)
            {
                controllerStatus = true;
            }

            myFunctions.SetMyConnectedObjects(connectedGameObjs, turretStatus);
        }

        private void Update()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {           
            turretLocomotion.currentTarget = fieldOfView.currentTarget;

            if (fieldOfView.currentTarget == null && !isRotDefault)
            {               
                turretLocomotion.SetDefault();
                isRotDefault = true;
            }
            else if(fieldOfView.currentTarget!=null)
            {
                turretLocomotion.myLight.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                turretLocomotion.myLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                anim.SetBool("isUp", true);
                
                if(fieldOfView.viewMeshRenderer!=null)
                {
                    fieldOfView.viewMeshRenderer.material.SetColor("_BaseColor", new Color(1, 0, 0, 0.25f));
                    fieldOfView.viewMeshRenderer.material.SetColor("_EmissionColor", new Color(1, 0, 0, 0.25f));
                }

                turretLocomotion.StopAllCoroutines();
                turretLocomotion.HandleRotateTowardsTarget();
                isRotDefault = false;
                turretStatus = false;
                isAreaEmpty = false;
                DenemeASD(false);
            }

        }

        public void PressedButton(bool isButtonOn)
        {
            //controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);
        }

        public void DenemeASD(bool getBool)
        {
            if (getBool)
            {
                turretStatus = true;
                myFunctions.SetMyConnectedObjects(connectedGameObjs, true);             
            }
            else if(!getBool)
            { 
                myFunctions.SetMyConnectedObjects(connectedGameObjs, false);                
            }
        }
    }
}