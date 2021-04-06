using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserGenerator : MonoBehaviour , IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool laserGenStatus = false;
        public bool myStatus => laserGenStatus;

        IReflectable _iReflectable;
        IReflectable iReflectable;
        private LineRenderer lineRenderer;
        private Ray ray;
        private RaycastHit hit;
        private float remaininLength;

        [SerializeField] private float maxLength;

        [Header("Requirement Type")]                                          //REQUIREMENT TYPE
        [SerializeField] private RequirementTypes.LaserReqTypes laserReqType;    //

        [Header("Bridge Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Connected Objects And Butons")]
        [SerializeField] private GameObject[] controllerObjs;   // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;                   // CHECK ALL CONNECTED BUTTONS      

        private void Start()
        {
            if(controllerObjs.Length<=0)
            {
                laserGenStatus = true;
            }

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;

            switch (laserReqType) //update içine alýnabilir hatta renk deðiþtirme için farklý yok bulunabilir (cube bir yere koy rengini kýrýmýzý yapvs)
            {
                case RequirementTypes.LaserReqTypes.nothing:
                    lineRenderer.material.SetColor("_BaseColor", Color.white);
                    lineRenderer.material.SetColor("_EmissionColor", Color.white);
                    break;
                case RequirementTypes.LaserReqTypes.RedLaser:
                    lineRenderer.material.SetColor("_BaseColor", Color.red);
                    lineRenderer.material.SetColor("_EmissionColor", Color.red);
                    break;
                case RequirementTypes.LaserReqTypes.GreenLaser:
                    lineRenderer.material.SetColor("_BaseColor", Color.green);
                    lineRenderer.material.SetColor("_EmissionColor", Color.green);
                    break;
                case RequirementTypes.LaserReqTypes.BlueLaser:
                    lineRenderer.material.SetColor("_BaseColor", Color.cyan);
                    lineRenderer.material.SetColor("_EmissionColor", Color.cyan);
                    break;
            }
        }

        void Update()
        {   
            if(laserGenStatus)
            {
                lineRenderer.enabled = true;
                ray = new Ray(transform.position, transform.forward);
                float remaininLength = maxLength;
                lineRenderer.SetPosition(0, transform.position);

                if (Physics.Raycast(ray.origin, ray.direction, out hit, remaininLength))
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remaininLength -= Vector3.Distance(ray.origin, hit.point);

                    iReflectable = hit.transform.GetComponent<IReflectable>();

                    if (iReflectable != null)
                    {
                        if (ContainsLaser(iReflectable.laserReqType))
                        {
                            iReflectable.Reflect(true, maxLength);
                            CheckOld();
                            _iReflectable = iReflectable;
                        }
                        else
                        {
                            iReflectable.Reflect(false, maxLength);
                            CheckOld();
                        }
                    }
                    else
                    {
                        CheckOld();
                    }
                }
            }
            else
            {
                lineRenderer.enabled = false;
                SetOffReflectors();
            }            
        }

        private bool ContainsLaser(RequirementTypes.LaserReqTypes getLaserType)
        {
            return laserReqType == getLaserType;
        }

        private void CheckOld()
        {
            if (_iReflectable != null)
            {
                if (iReflectable != _iReflectable)
                {
                    SetOffReflectors();
                }
            }
        }
        
        private void SetOffReflectors()
        {
            if (_iReflectable != null)
            {
                _iReflectable.Reflect(false, 0);
                _iReflectable = null;
            }
        }

        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if(controllerStatus)
            {
                laserGenStatus = controllerStatus;

                switch (myLogicGateType) //daha iyi bulunursa silinir
                {
                    case MyFunctions.LogicGateType.DontHaveGate:
                        laserGenStatus = isButtonOn;
                        break;
                    case MyFunctions.LogicGateType.Not:
                        laserGenStatus = !isButtonOn;
                        break;
                }
            }
            else if (!controllerStatus)
            {
                laserGenStatus = false;
                controllerStatus = false;
            }
        }
    }
}