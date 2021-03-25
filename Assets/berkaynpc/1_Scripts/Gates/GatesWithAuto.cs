using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class GatesWithAuto : MonoBehaviour , IHold
    {
       
        [SerializeField] 
        private bool isGateOpen = false;

        [SerializeField]
        private bool areaEmpty = true;

        [SerializeField] private Animator thisAnimator;

        [Header("Key Type")]
        [SerializeField] private RequirementTypes.RequirementType reqType;
        RequirementTypes.RequirementType IHold.reqType => reqType;
        public bool myStatus => true;

        private bool AnimatorIsPlaying()
        {
            return thisAnimator.GetCurrentAnimatorStateInfo(0).length >
                   thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        private void Awake()
        {
            thisAnimator = GetComponent<Animator>(); 
        }

        private void Update()
        {
            if(areaEmpty && isGateOpen && !AnimatorIsPlaying())
            {
                CmdOpenDoor(false);
            }
        }

        public void CmdOpenDoor(bool getDoor)
        {
            RpcOpenDoor(getDoor);
        }

        private void RpcOpenDoor(bool getDoor)
        {
            isGateOpen = getDoor;          
            thisAnimator.SetBool("Open", isGateOpen); //yeni baðlanan biri kapý açýksa kapalý görüyor *** bunu düzelt ******************************
        }


        public void Holding()
        {
            areaEmpty = false;

            if (!isGateOpen && !AnimatorIsPlaying() && !areaEmpty)
            {
                CmdOpenDoor(true);
            }
        }

        public void AreaEmpty()
        {
            areaEmpty = true;
        }
    }
}