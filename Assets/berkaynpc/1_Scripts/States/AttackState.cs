using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class AttackState : StateBase
    {
        public override StateBase RunCurrentState()
        {
            Debug.Log("I Have Attacked!");
            return this;
        }
    }
}