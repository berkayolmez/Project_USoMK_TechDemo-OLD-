using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    public abstract class StateManager : MonoBehaviour
    {
        State currentState;
        Dictionary<string, State> allStates = new Dictionary<string, State>();

        [HideInInspector]
        public Transform mTransform;

        private void Start()
        {
            mTransform = this.transform;

            Init();
        }

        public abstract void Init();
          
        public void Tick()
        {
            if(currentState==null)
            {
                return;
            }

            currentState.Tick();
        } //update 

        public void FixedTick()
        {
            if (currentState == null)
            {
                return;
            }

            currentState.FixedTick();
        } //fixedupdate

        public void LateTick()
        {
            if (currentState == null)
            {
                return;
            }

            currentState.LateTick();
        } //lateupdate

        public void ChangeState(string targetId)
        {
            if(currentState !=null)
            {
                //run on exit acions of currentstate
            }

            State targetState = GetState(targetId);
            currentState = targetState; 
            //run on enter actions
        }

        State GetState(string targetId)
        {
            allStates.TryGetValue(targetId, out State retVal);
            return retVal;
        }

        protected void RegisterState(string stateId,State state) //?????
        {
            allStates.Add(stateId, state);
        }

    }
}