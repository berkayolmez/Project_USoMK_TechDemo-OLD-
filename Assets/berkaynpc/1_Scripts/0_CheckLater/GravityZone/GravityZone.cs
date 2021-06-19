using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class GravityZone : MonoBehaviour
    {
        public float gravityForce = 1;
        public float ballDist = 0;
        public float oldGravity;
        public float oldGravityY;

        private void OnTriggerEnter(Collider other)
        {
            PlayerLocomotion playerLoc = other.GetComponent<PlayerLocomotion>();

            if (playerLoc != null)
            {
                oldGravity = playerLoc.gravity;
                oldGravityY = playerLoc.gravityY;
                playerLoc.gravityY = gravityForce;
                playerLoc.gravity = gravityForce;
                //playerLoc.HandleAddImpact(Vector3.up, gravityForce);
            }
        }

        private void OnTriggerStay(Collider other)
        {
          
            /*
            Rigidbody getRigid = other.GetComponent<Rigidbody>();
            if (getRigid != null)
            {
                ballDist = Vector3.Distance(transform.position, other.transform.position);
                getRigid.AddForce(transform.forward * gravityForce / ballDist, ForceMode.Impulse);
            }*/
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerLocomotion playerLoc = other.GetComponent<PlayerLocomotion>();

            if (playerLoc != null)
            {
                playerLoc.gravity = oldGravity;
                playerLoc.gravityY = oldGravityY ;
                //playerLoc.HandleAddImpact(Vector3.up, gravityForce);
            }
        }

    }
}