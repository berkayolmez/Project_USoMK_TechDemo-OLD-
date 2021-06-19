using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserGenerator : ObjectBase
    {
        IReflectable _iReflectable;
        IReflectable iReflectable;
        private LineRenderer lineRenderer;
        private Ray ray;
        private RaycastHit hit;

        [Header("Laser Requirement Type")]                                          
        [SerializeField] private RequirementTypes.LaserReqTypes laserReqType;       //Generatable laser type

        [Header("Laser Generator Settings")]
        [SerializeField] private float maxLaserLength;       //Max Laser length

        protected override void Start()
        {
            base.Start();
            StartCoroutine("WaitForStart");

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;

            switch (laserReqType) //This can be moved into Update. //Find different ways to change color
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

        IEnumerator WaitForStart()
        {
            yield return new WaitForSeconds(0.1f);

            if (myControllerObjList.Count <= 0)
            {
                myControllerStatus = true;
                myCurrentStatus = true;                
            }
        }

        void Update()
        {   
            if(myCurrentStatus)
            {
                lineRenderer.enabled = true;
                ray = new Ray(transform.position, transform.forward);
                float remaininLength = maxLaserLength;
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
                            iReflectable.Reflect(true, maxLaserLength);
                            CheckOld();
                            _iReflectable = iReflectable;
                        }
                        else
                        {
                            iReflectable.Reflect(false, maxLaserLength);
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

        public override void OnSettingMe(string getID, bool isButtonOn)
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
                            myCurrentStatus = isButtonOn;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !isButtonOn;
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
    }
}