using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class UI_Listener : ObserverBase
    {
        protected override void Start()
        {
            base.Start();
            StartCoroutine("WaitForFindControllers");
        }

        private IEnumerator WaitForFindControllers()
        {
            yield return new WaitForSeconds(0.1f);
            OnSettingMe(myID, false);
            yield break;
        }

        public override void OnSettingMe(string getID, bool getBool)
        {
            if (getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, MyFunctions.LogicGateType.DontHaveGate);

                if (myControllerStatus)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}