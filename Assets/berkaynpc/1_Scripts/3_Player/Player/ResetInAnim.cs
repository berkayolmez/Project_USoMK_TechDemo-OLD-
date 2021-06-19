using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInAnim : StateMachineBehaviour
{
    [System.Serializable]
    public struct BoolStatus
    {
        public string targetBool;
        public bool status;
    }

    public BoolStatus[] boolStatuses;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)       //To reset the animation
    {
        foreach (BoolStatus b in boolStatuses)
        {
            animator.SetBool(b.targetBool, b.status);
        }
    }
}

