using TMPro;
using System.Collections;
using UnityEngine;

namespace project_usomk
{
    /// <summary>
    /// The color of the text will be green if the status of the object is true, and red if it is false.
    /// </summary>
    public class TestOnOffUIController : ControllerBase
    {
        [Header("Button Settings")]
        [SerializeField] private TextMeshProUGUI thisTextUI;

        protected override void Start()
        {
            base.Start();

            thisTextUI = GetComponent<TextMeshProUGUI>();

            if (myCurrentStatus)
            {
                thisTextUI.color = Color.green;
            }
            else
            {
                thisTextUI.color = Color.red;
            }

            StartCoroutine("WaitForFindControllers");         
        }

        private IEnumerator WaitForFindControllers()
        {
            yield return new WaitForSeconds(0.1f);
            OnSettingMe(myID, false);
            yield break;
        }

        public override void OnSettingMe(string getID,bool getBool)
        {
            if(getID == this.myID)
            {
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    myCurrentStatus = myControllerStatus;

                    switch (myLogicGateType)
                    {
                        case MyFunctions.LogicGateType.DontHaveGate:
                            myCurrentStatus = getBool;
                            break;
                        case MyFunctions.LogicGateType.Not:
                            myCurrentStatus = !getBool;
                            break;
                    }

                    if (myCurrentStatus)
                    {
                        thisTextUI.color = Color.green;         //Set text color to green
                    }
                    else
                    {
                        thisTextUI.color = Color.red;           //Set text color to red
                    }
                }
                else
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    thisTextUI.color = Color.red;
                }
            }            
        }
    }
}