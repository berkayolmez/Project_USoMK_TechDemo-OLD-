using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class ChaseState : StateBase
    {
        public AttackState attackState;
        public bool isInAttackRange;

        public override StateBase RunCurrentState()
        {
            if(isInAttackRange)
            {
                return attackState;
            }
            else
            {
                return this;
            }
        }
    }
}