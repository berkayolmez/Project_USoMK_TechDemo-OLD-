using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace project_WAST
{
    public class TurretLocomotion : MonoBehaviour
    {        
        Turrets turretManager;
        FieldOfView fieldOfView;
        public GameObject myLight;
        public Transform currentTarget;
        [SerializeField] private float rotationSpeed=1;
        [SerializeField] private float resetSpeed=3;
        [Range(0,180)]
        [SerializeField] private float resetAngle = 0;

        private Quaternion startedRot;
        private Quaternion lostRot1;
        private Quaternion lostRot2;

        public bool isStarted=true;

        private int count = 0;
        [SerializeField] private int maxResetSearch = 1;
        
        private void Start()
        {
            fieldOfView = GetComponent<FieldOfView>();
            turretManager = GetComponent<Turrets>();
            myLight.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
            myLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);   
            startedRot = transform.rotation;
        }

        public void SetDefault()
        {
          StartCoroutine(TargetLost());           
        }

        public void HandleRotateTowardsTarget()
        {            
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }

        public IEnumerator TargetLost()
        {
         //   resetAngle = resetAngle / 360;
            myLight.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.yellow);
            myLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.yellow);

            if (fieldOfView.viewMeshRenderer != null)
            {
                fieldOfView.viewMeshRenderer.material.SetColor("_BaseColor", new Color(1, 0.92f, 0.016f, 0.25f));
                fieldOfView.viewMeshRenderer.material.SetColor("_EmissionColor", new Color(1, 0.92f, 0.016f, 0.25f));
            }

            yield return new WaitForSeconds(1f);

           lostRot1 = transform.rotation*Quaternion.Euler(0, -resetAngle, 0);
           lostRot2 = transform.rotation * Quaternion.Euler(0,resetAngle,0);
           Quaternion newRot = lostRot1;

            count = 0;
            while(count< maxResetSearch)
            {  
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, resetSpeed);       
                
                if(transform.rotation==newRot)
                {
                    if(newRot==lostRot1)
                    {
                        newRot = lostRot2;
                        count++;
                    }
                    else if (newRot==lostRot2)
                    {
                        newRot = lostRot1;
                        count++;
                    }
                }
                yield return null;
            }

            count = 0;

            myLight.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.green);
            myLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);


            if (fieldOfView.viewMeshRenderer != null)
            {
                fieldOfView.viewMeshRenderer.material.SetColor("_BaseColor", new Color(0, 1, 0, 0.25f));
                fieldOfView.viewMeshRenderer.material.SetColor("_EmissionColor", new Color(0, 1, 0, 0.25f));
            }

            turretManager.DenemeASD(true);
            turretManager.isAreaEmpty = true;
            turretManager.anim.SetBool("isUp", false);

            while (transform.rotation != startedRot)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startedRot, rotationSpeed);

                yield return null;
            }

            yield break;           
        }

    }
}