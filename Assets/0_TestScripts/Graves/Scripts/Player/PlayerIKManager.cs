using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

namespace project_WAST
{
    public class PlayerIKManager : MonoBehaviour
    {
        PlayerAnimatorManager playerAnimManager;

        [SerializeField] private float maxHitDist=5f;
        [SerializeField] private float heightOffset = 3f;
        [SerializeField] private bool[] allGroundCastHits;
        private LayerMask hitLayer;
        private Vector3[] allHitNormals;
        [SerializeField] private float angleX;
        [SerializeField] private float angleZ;
        private float[] allFootWeights;
        [SerializeField] private float yOffset = 0.15f;
        [SerializeField] private float castRadius = 0.2f;

        private Vector3 castOrigin;

        LayerMask groundLayerMask;

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

        private void Awake()
        {
            playerAnimManager = GetComponentInChildren<PlayerAnimatorManager>();
        }


        private void Start()
        {
            allFootTransforms = new Transform[2];
            allFootTransforms[0] = rightFoot;
            allFootTransforms[1] = leftFoot;

            allFootTargets = new Transform[2];
            allFootTargets[0] = rightFootTar;
            allFootTargets[1] = leftFootFTar;

            allFootIKConstrains = new TwoBoneIKConstraint[2];
            allFootIKConstrains[0] = rightFootRig.GetComponent<TwoBoneIKConstraint>();
            allFootIKConstrains[1] = leftFootrig.GetComponent<TwoBoneIKConstraint>();

            groundLayerMask = LayerMask.NameToLayer("Ground");

            allGroundCastHits = new bool[3];
            allHitNormals = new Vector3[2];

            allFootWeights = new float[2];
        }


        private void FixedUpdate()
        {
            RotateCharacterFeet();
        }

        private void CheckGround(out Vector3 hitPoint, out bool gotGroundCastHit, out Vector3 hitNormal, out LayerMask hitLayer, out float currentHitDist, Transform objTransform, int checkForLayerMask, float maxHitDist, float heightOffset)
        {            
            RaycastHit hit;
            castOrigin = objTransform.position + new Vector3(0, heightOffset, 0);

            if(checkForLayerMask == -1)
            {
                Debug.LogError("Layer doesn't exist!");
                gotGroundCastHit = false;
                currentHitDist = 0f;
                hitLayer = LayerMask.NameToLayer("Player");
                hitNormal = Vector3.up;
                hitPoint = objTransform.position;
            }
            else
            {
                int layerMask = (1 << checkForLayerMask);
                if (Physics.SphereCast(castOrigin, castRadius, Vector3.down, out hit, maxHitDist, layerMask, QueryTriggerInteraction.UseGlobal))
                {
                    hitLayer = hit.transform.gameObject.layer;
                    currentHitDist = hit.distance - heightOffset;
                    hitNormal = hit.normal;
                    gotGroundCastHit = true;
                    hitPoint = hit.point;
                }
                else
                {
                    gotGroundCastHit = false;
                    currentHitDist = 0f;
                    hitLayer = LayerMask.NameToLayer("Player");
                    hitNormal = Vector3.up;
                    hitPoint = objTransform.position;
                }
            }
        }

        Vector3 ProjectOnSurface(Vector3 vector,Vector3 hitNormal)
        {
            return vector - hitNormal * Vector3.Dot(vector, hitNormal);
        }

        private void ProjectedAxisAngle(out float angleX,out float angleZ,Transform footTargetTransform,Vector3 hitNormal)
        {
            Vector3 xAxisProject = ProjectOnSurface(footTargetTransform.forward, hitNormal).normalized;
            Vector3 zAxisProject = ProjectOnSurface(footTargetTransform.right, hitNormal).normalized;

            angleX = Vector3.SignedAngle(footTargetTransform.forward, xAxisProject, footTargetTransform.right);
            angleZ = Vector3.SignedAngle(footTargetTransform.right, zAxisProject, footTargetTransform.forward);
        }

        private void RotateCharacterFeet()
        {
            allFootWeights[0] = playerAnimManager.animator.GetFloat("RightFootWeight");
            allFootWeights[1] = playerAnimManager.animator.GetFloat("LeftFootWeight");

            for (int i = 0; i < 2; i++) //4 yerine part sayýsý gelecek***** 
            { 
                allFootIKConstrains[i].weight = allFootWeights[i];

                CheckGround(out Vector3 hitPoint, out allGroundCastHits[i], out Vector3 hitNormal, out hitLayer, out _, allFootTransforms[i],groundLayerMask,maxHitDist,heightOffset);
                allHitNormals[i] = hitNormal;

                if(allGroundCastHits[i])
                {
                    if(allFootTransforms[i].position.y<allFootTargets[i].position.y-0.1f)
                    {
                        yOffset += allFootTargets[i].position.y - allFootTransforms[i].position.y - 0.1f;
                    }

                    ProjectedAxisAngle(out angleX, out angleZ, allFootTransforms[i], allHitNormals[i]);
                    allFootTargets[i].position = new Vector3(allFootTransforms[i].position.x, hitPoint.y + yOffset, allFootTransforms[i].position.z);
                    allFootTargets[i].rotation = allFootTransforms[i].rotation;
                    allFootTargets[i].localEulerAngles = new Vector3(allFootTargets[i].localEulerAngles.x + angleX, allFootTargets[i].localEulerAngles.y, allFootTargets[i].localEulerAngles.z + angleZ);

                }    
                else
                {
                    allFootTargets[i].position = allFootTransforms[i].position;
                    allFootTargets[i].rotation = allFootTransforms[i].rotation;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(castOrigin, 0.2f);
        }

    }

}

