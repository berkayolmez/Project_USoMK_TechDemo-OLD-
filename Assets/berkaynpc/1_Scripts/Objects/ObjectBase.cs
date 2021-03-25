using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class ObjectBase : MonoBehaviour,IHaveButton,IHaveStatus
    {
        private MyFunctions myFunctions = new MyFunctions();  
              
        public bool buttonStatus = true;
        public bool myStatus => buttonStatus;           

        public GameObject[] connectedGameObjs;
        public GameObject[] controllerObjs;
        public bool controllerStatus;   

        public virtual void PressedButton(bool isButtonOn)
        {
          
        }
    }
}