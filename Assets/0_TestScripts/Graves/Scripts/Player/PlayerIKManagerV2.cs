using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace project_WAST
{
    public class PlayerIKManagerV2 : MonoBehaviour
    {
        public float offSet;
        public float rayDist;
        public float surfaceAngle;
        public LayerMask layerMask;

        [Header("Character Body Parts")]
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform leftFoot;
        private Transform[] allFootTransforms;

        [Header("Rig Targets")] //targetler gameobjeden çekilebilir****
        [SerializeField] private Transform rightFootTar;
        [SerializeField] private Transform leftFootFTar;
        private Transform[] allFootTargets;

        [Header("Rigs")]
        [SerializeField] private GameObject rightFootRig;
        [SerializeField] private GameObject leftFootrig;
        TwoBoneIKConstraint[] allFootIKConstrains;

        Ray ray;
        RaycastHit rayHit;

        private void FixedUpdate()
        {            
            Vector3 newPos = new Vector3(rightFoot.position.x, rightFoot.position.y+ offSet, rightFoot.position.z);          
            Debug.DrawRay(newPos, -transform.up* rayDist, Color.red);
            if (Physics.Raycast(newPos, -transform.up, out rayHit, rayDist, layerMask))
            {
                surfaceAngle = Vector3.Angle(rayHit.normal, transform.forward);
                Vector3 surfaceParallel = transform.forward - rayHit.normal* Vector3.Dot(transform.forward, rayHit.normal);
                Debug.DrawRay(rayHit.point,rayHit.normal, Color.green);
                Debug.DrawRay(rayHit.point, surfaceParallel, Color.blue);               
            }
        }
        private void Update()
        {
            rightFootTar.localEulerAngles = new Vector3(surfaceAngle+90, rightFootTar.localEulerAngles.y, rightFootTar.localEulerAngles.z);
        }

        private void CheckSurface()
        {

        }

        void OnDrawGizmosSelected()
        {
          
        }
    }
}