using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class EnemyManager : MonoBehaviour
    {
        EnemyLocomotionManager enemyLocomotionManager;

        [Header("Select a AI Type")]
        [SerializeField] EnemyAI_Type ai_Type;
        public enum EnemyAI_Type
        {
            SimpleTurret,
            SimpleBug,
        }

        public EnemyAI_Type AI_Type => ai_Type;

        public bool isPerformingAction;

        [Header("Settings")]
        public float detectionRadius=20;
        //basit oalrak fov yaptýk
        public float maxDetectionAngle=50;
        public float minDetectionAngle=-50;

        private void Awake()
        {
            enemyLocomotionManager=GetComponent<EnemyLocomotionManager>();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            switch(ai_Type)
            {
                case EnemyAI_Type.SimpleTurret:

                    if (enemyLocomotionManager.aaacurrentTarget == null)
                    {

                        enemyLocomotionManager.HandleDetection();
                    }
                    else
                    {
                        enemyLocomotionManager.HandleRotateTowardsTarget();
                    }

                    break;

                case EnemyAI_Type.SimpleBug:

                    if (enemyLocomotionManager.aaacurrentTarget == null)
                    {
                        enemyLocomotionManager.HandleDetection();
                        //back to cave
                    }
                    else
                    {
                       // if()
                       enemyLocomotionManager.HandleMoveToTarget();

                    }

                    break;
            }

            /*if(enemyLocomotionManager.currentTarget ==null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }*/
        }

    }
}