using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace project_WAST
{
    public class IdleState : StateBase
    {
        public ChaseState chaseState;
        public bool canSeeThePlayer;

        public override StateBase RunCurrentState()
        {
            if(canSeeThePlayer)
            {
                return chaseState;
            }
            else
            {
                return this;
            }
        }
    }
}