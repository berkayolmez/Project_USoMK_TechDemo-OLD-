using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class EnemyAnimationManager : AnimationManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        public Vector3 deltaPos;
        public Vector3 velocity;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigid.drag = 0;
            deltaPos = anim.deltaPosition;
            deltaPos.y = 0;
            velocity = deltaPos / delta;
            enemyLocomotionManager.enemyRigid.velocity = velocity;
        }
    }
}