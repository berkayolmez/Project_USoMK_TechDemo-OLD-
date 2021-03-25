using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class GatesWithButton : MonoBehaviour , IHaveButton
    {

        private MyFunctions myFunctions = new MyFunctions();
        private Animator thisAnimator;

        [Header("My Status (True/False)")]
        [SerializeField] private bool controllerStatus;
        [SerializeField] private bool gateStatus;

        [Header("Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        private void Awake()
        {
            thisAnimator = GetComponent<Animator>();
        }
        
        private IEnumerator WaitForTimer()         //WAIT FOR canPressTimer
        {
            thisAnimator.SetBool("Open", gateStatus);
            yield return new WaitForSeconds(thisAnimator.GetCurrentAnimatorStateInfo(0).length); //delayli açýlýyor bunu düzelttttttt******
                               
            StopCoroutine("WaitForTimer");                           //Stop coroutine
        }     


        public void PressedButton(bool isButtonOn)
        {
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);

            if (controllerStatus)
            {
                gateStatus = isButtonOn;
                StartCoroutine("WaitForTimer");
            }
            else
            {
                gateStatus = false;
                StartCoroutine("WaitForTimer");
            }
        }

            
    }
}