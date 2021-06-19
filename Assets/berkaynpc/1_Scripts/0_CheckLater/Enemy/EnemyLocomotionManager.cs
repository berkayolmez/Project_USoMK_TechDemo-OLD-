using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace project_usomk
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimationManager enemyAnimManager;
        NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigid;

        public CharacterStats charStats;
        public CharacterStats aaacurrentTarget;
        public Transform currentTarget;
        public LayerMask detectionLayer;

        public float distanceFromTarget;
        public float stoppingDistance = 1f;

        public float rotationSpeed = 15;

        //[HideInInspector]
        public List<Transform> visibleTargets = new List<Transform>();

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimManager = GetComponentInChildren<EnemyAnimationManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigid = GetComponent<Rigidbody>();
        }


        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigid.isKinematic = false;
        }

        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if(characterStats!=null)
                {
                    //check for team id

                    Vector3 targetDir = characterStats.transform.position-transform.position; //target yonunu hesapladi
                    float viewableAngle = Vector3.Angle(targetDir, transform.forward);  //gorus acisini hesapladi

                    if(viewableAngle>enemyManager.minDetectionAngle&&viewableAngle<enemyManager.maxDetectionAngle)
                    {
                        aaacurrentTarget = characterStats; //char yerine ITargetable olmalý gibi
                    }
                }
            }
        
        }

        public void HandleMoveToTarget()
        {
            Vector3 targetDir = aaacurrentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(aaacurrentTarget.transform.position, transform.position);
            float viewAngle = Vector3.Angle(targetDir, transform.forward);

            //eger bir aksiyondaysak hareketi ve navmeshi kapat
            if(enemyManager.isPerformingAction)
            {
               enemyAnimManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
               navMeshAgent.enabled = false;
            }
            else
            {
                if(distanceFromTarget>stoppingDistance)
                {
                    enemyAnimManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                    enemyRigid.velocity = targetDir * rotationSpeed;
                }
                else if(distanceFromTarget<=stoppingDistance)
                {
                    enemyAnimManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }

            HandleRotateTowardsTarget();

        

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        public void HandleRotateTowardsTarget()
        {
            //rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = aaacurrentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
            }
            else //rotate with pathfinding(navmesh)
            {
                Vector3 relativeDir = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigid.velocity;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(aaacurrentTarget.transform.position);
                enemyRigid.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }

            /*
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;*/
        }
    }
}
