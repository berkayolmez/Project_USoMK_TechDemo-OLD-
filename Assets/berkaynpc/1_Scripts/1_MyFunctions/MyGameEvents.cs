using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class MyGameEvents : MonoBehaviour
    {
        public static MyGameEvents current;
        //public int target = 144; //for fps lock
        private void Awake()
        {
            current = this;

            //QualitySettings.vSyncCount = 0;  //for fps lock
            //Application.targetFrameRate = target;    //for fps lock
        }
        void Update()
        {
            // if (Application.targetFrameRate != target)  //for fps lock
            // Application.targetFrameRate = target; //for fps lock
        }


        public event Action<string,GameObject> onImYourController;
        public void ImYourController(string id,GameObject controller)
        {
            if (onImYourController != null)
            {
                onImYourController(id, controller);
            }
        }

        public event Action<string> onSendMyIDToListeners;
        public void SendMyIDtoListeners(string id)
        {
            if (onSendMyIDToListeners != null)
            {
                // Debug.Log("gameeventgelen id " + id +" gonderen "+ name);
                onSendMyIDToListeners(id);
            }
        }

        public event Action<string,bool> onSetTarget;
        public void SetTarget(List<string> getIDS,bool getBool)
        {
            if (onSetTarget != null)
            {
                foreach(string id in getIDS )
                {
                    onSetTarget(id, getBool);
                }
            }
        }

        public event Action<string, float,float> onSetSignal;
        public void SetSignal(List<string> getIDS, float getMySignal,float getMyMaxSignal)
        {
            if (onSetSignal != null)
            {
                foreach (string id in getIDS)
                {
                    onSetSignal(id, getMySignal,getMyMaxSignal);
                }
            }
        }



    }
}