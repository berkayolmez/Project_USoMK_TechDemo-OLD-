using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class WindTurbine : MonoBehaviour
    {
        public float windForce=1;
        public float ballDist = 0;

        private void OnTriggerStay(Collider other)
        {
            Rigidbody getRigid = other.GetComponent<Rigidbody>();
            if(getRigid!=null)
            {
                ballDist = Vector3.Distance(transform.position, other.transform.position);
                getRigid.AddForce(transform.forward * windForce / ballDist, ForceMode.Impulse);
            }
        }
    }
}