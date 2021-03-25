using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    public class State
    {
        bool forceExit;

        List<StateAction> fixedUpdateActions;
        List<StateAction> updateActions;
        List<StateAction> lateUpdateActions;

        public State(List<StateAction> fixedUpdateActions, List<StateAction> updateActions, List<StateAction> lateUpdateActions)
        {
            this.fixedUpdateActions = fixedUpdateActions;
            this.updateActions = updateActions;
            this.lateUpdateActions = lateUpdateActions;
        }

        public void FixedTick()
        {
            ExecuteListOfActions(fixedUpdateActions);
        }

        public void Tick()
        {
            ExecuteListOfActions(updateActions);
        }

        public void LateTick()
        {
            ExecuteListOfActions(lateUpdateActions);
            forceExit = false;
        }

        void ExecuteListOfActions(List<StateAction> actionList)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                if(forceExit)
                {
                    return;
                }

                forceExit=actionList[i].Execute();
            }
        }

    }
}