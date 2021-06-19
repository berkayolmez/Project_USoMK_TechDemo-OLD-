using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class Turrets : ControllerBase
    {    
        TurretLocomotion turretLocomotion;
        FieldOfView fieldOfView;
        public Animator anim;

        public bool isRotDefault = true;
        public bool isAreaEmpty=true;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            turretLocomotion = GetComponent<TurretLocomotion>();
            fieldOfView = GetComponent<FieldOfView>();
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine("WaitForStart");
        }

        IEnumerator WaitForStart()
        {
            yield return new WaitForSeconds(0.1f);
            if (myControllerObjList.Count <= 0)
            {
                myControllerStatus = true;
                myCurrentStatus = true;
            }
            MyGameEvents.current.SetTarget(targetID, myStatus);
        }

        private void Update()
        {
            OnCurrentAction();      //Turret action
        }

        private void OnCurrentAction()
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
                myCurrentStatus= false;
                isAreaEmpty = false;
                InteractedMe(false);
            }
        }

        public override void InteractedMe(bool getBool)
        {
            if (getBool)
            {
                myCurrentStatus = true;
                MyGameEvents.current.SetTarget(targetID, true);
            }
            else if(!getBool)
            {
                MyGameEvents.current.SetTarget(targetID, false);
            }
        }

        public override void OnSettingMe(string getTargetID, bool getbool)      //Will be updated. "myControllerStatus" etc.
        {
           
        }
    }
}