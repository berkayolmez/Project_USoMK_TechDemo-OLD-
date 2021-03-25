using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class Object_Light : ObjectBase
    {
        private Light thisLight;
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool lightStatus = true;
        public bool objStatus => myStatus;

        public override void PressedButton(bool isButtonOn)
        {
            base.PressedButton(isButtonOn);
            

        }


    }
}