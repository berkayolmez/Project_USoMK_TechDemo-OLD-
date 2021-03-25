using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class StateManager : MonoBehaviour
    {
        StateBase currentState;

        private void Update()
        {
            RunStateMachine();
        }

        private void RunStateMachine()
        {
            StateBase nextState = currentState?.RunCurrentState();

            if(nextState!=null)
            {
                SwitchToNextState(nextState);
            }
        }

        private void SwitchToNextState(StateBase nextState)
        {
            currentState = nextState;
        }
    }
}