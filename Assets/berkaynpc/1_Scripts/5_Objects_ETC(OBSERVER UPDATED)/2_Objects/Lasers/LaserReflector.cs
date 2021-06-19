using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserReflector : MonoBehaviour,IReflectable    
    {
        private List<IReflectable> _iReflectable=new List<IReflectable>();
        IReflectable iReflectable;

        private LineRenderer lineRenderer;
        private Ray ray;
        private RaycastHit hit;
        private List<Vector3> directions = new List<Vector3>();
        private int rayCount = 1;
        private int currentCount = 0;

        #region Reflector Type
        [Header("Reflector Type")]                                  
        [SerializeField] ReflectorTypes reflectorType;
        public enum ReflectorTypes                                
        {
           ReflectorSimple,
           ReflectorDual,
           ReflectorTriple,
           ReflectorQuadro,
           ReflectorTriple90Degre,

        }
        public ReflectorTypes GetReflectorTypes() => reflectorType;
        #endregion

        #region Reflectable Laser Type

        [Header("Requirement Type")]                                           //Reflectable laser type  
        [SerializeField] private RequirementTypes.LaserReqTypes laserReqType;    
        RequirementTypes.LaserReqTypes IReflectable.laserReqType => laserReqType;    // GET MY TYPE TO INTERFACE

        #endregion
      
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _iReflectable.Insert(0, null);
            _iReflectable.Insert(1, null);
            _iReflectable.Insert(2, null);
            _iReflectable.Insert(3, null);
            _iReflectable.Insert(4, null);
            _iReflectable.Insert(5, null);
        }

        public void Reflect(bool getBool,float getLaserLength)
        {
            if (getBool)
            {
                lineRenderer.enabled = true;

                switch (laserReqType)
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
                    
                switch (reflectorType)
                {
                    case ReflectorTypes.ReflectorSimple:

                        rayCount = 1;
                        
                            if(directions.Count<=1)
                            {
                               directions.Insert(0, transform.forward);
                            }
                            else if(directions.Count>1)
                            {
                                directions.Clear();
                                directions.Insert(0, transform.forward);
                            }                       
                        
                        break;

                    case ReflectorTypes.ReflectorDual:
                        rayCount = 2;

                        if (directions.Count <= 2)
                        {
                            directions.Insert(0, transform.forward + transform.right*-0.5f);
                            directions.Insert(1, transform.forward + transform.right *0.5f);
                        }
                        else if (directions.Count > 2)
                        {
                            directions.Clear();
                            directions.Insert(0, transform.forward + transform.right * -0.5f);
                            directions.Insert(1, transform.forward + transform.right * 0.5f);
                        }
                        
                        break;


                    case ReflectorTypes.ReflectorTriple:
                        rayCount = 3;

                        if (directions.Count <= 3)
                        {
                            directions.Insert(0,transform.forward + transform.right * -0.5f);
                            directions.Insert(1,transform.forward);
                            directions.Insert(2,transform.forward + transform.right * 0.5f);
                        }
                        else if (directions.Count > 3)
                        {
                            directions.Clear();
                            directions.Insert(0,transform.forward + transform.right * -0.5f);
                            directions.Insert(1,transform.forward);
                            directions.Insert(2,transform.forward + transform.right * 0.5f);
                        }
                        break;

                    case ReflectorTypes.ReflectorQuadro:
                        rayCount = 4;
                        if (directions.Count <= 4)
                        {
                            directions.Insert(0,transform.forward + transform.right * -0.7f);
                            directions.Insert(1,transform.forward + transform.right * -0.25f);
                            directions.Insert(2,transform.forward + transform.right * 0.25f);
                            directions.Insert(3,transform.forward + transform.right * 0.7f);
                        }
                        else if (directions.Count > 4)
                        {
                            directions.Clear(); 
                            directions.Insert(0, transform.forward + transform.right * -0.7f);
                            directions.Insert(1, transform.forward + transform.right * -0.25f);
                            directions.Insert(2, transform.forward + transform.right * 0.25f);
                            directions.Insert(3, transform.forward + transform.right * 0.7f);
                        }
                        break;

                    case ReflectorTypes.ReflectorTriple90Degre:
                        rayCount = 3;
                        if (directions.Count <= 3)
                        {
                            directions.Insert(0, -transform.right);
                            directions.Insert(1, transform.forward);
                            directions.Insert(2, transform.right);
                        }
                        else if (directions.Count > 3)
                        {
                            directions.Clear();
                            directions.Insert(0, -transform.right);
                            directions.Insert(1, transform.forward);
                            directions.Insert(2, transform.right);
                        }

                        break;
                }

                lineRenderer.positionCount = rayCount * 2;

                for (currentCount = 0; currentCount < rayCount; currentCount++)
                {
                    CheckTargets(getLaserLength,directions[currentCount], currentCount);
                }
            }
            else
            {    
                lineRenderer.enabled = false;
                StopALLConnected();
            }    
        }

        private void CheckTargets(float getLength,Vector3 getDirection,int getCurrentCount)
        {          
            ray = new Ray(transform.position, getDirection);
            float remaininLength = getLength;

            lineRenderer.SetPosition(getCurrentCount * 2,transform.position);          

            if (Physics.Raycast(ray.origin, ray.direction, out hit, remaininLength))
            {                
                lineRenderer.SetPosition((getCurrentCount * 2)+1, hit.point);
               
                remaininLength -= Vector3.Distance(ray.origin, hit.point);

                iReflectable = hit.transform.GetComponent<IReflectable>();

                if (iReflectable != null)
                {
                    if (ContainsLaser(iReflectable.laserReqType))
                    {  
                        CheckOld(getCurrentCount, iReflectable, getLength);
                        iReflectable.Reflect(true, getLength);
                    }
                    else
                    {
                        CheckOld(getCurrentCount, iReflectable, getLength);
                        iReflectable.Reflect(false, getLength);
                    }
                }
                else
                {
                  StopOldOne(getCurrentCount);
                }
            }          
        }

        private void StopOldOne(int getCurrentCount)
        {    
            if (_iReflectable[getCurrentCount] != null)
            {
                _iReflectable[getCurrentCount].Reflect(false, 0);
            }
        }

        private void StopALLConnected()
        {
            for (int i = 0; i < rayCount; i++)
            {
                if(_iReflectable[i]!=null)
                {
                    _iReflectable[i].Reflect(false, 0);
                    _iReflectable[i] = null;
                }
            }
        }

        private void CheckOld(int getCurrentCount,IReflectable getReflectable,float length)
        {
            if (_iReflectable[getCurrentCount] != getReflectable)
            {
                if(_iReflectable[getCurrentCount] !=null)
                {
                    _iReflectable[getCurrentCount].Reflect(false, 0);                  
                }
                _iReflectable.Insert(getCurrentCount, getReflectable);
            }
        }

        public bool ContainsLaser(RequirementTypes.LaserReqTypes getLaserType)
        {
            return laserReqType == getLaserType;
        }

    }     

    //My brain will explode. I can't explain this code. wow.
}