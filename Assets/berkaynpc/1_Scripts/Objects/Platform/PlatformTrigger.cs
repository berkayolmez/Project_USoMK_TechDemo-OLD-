using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace project_WAST
{
    public class PlatformTrigger : MonoBehaviour, IHaveStatus, IHaveButton
    {
        Renderer myRenderer;
        [SerializeField] private Collider myCollider;
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool platformTriggerStatus = true;
        public bool myStatus => platformTriggerStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Platform Type")]                               //CHOOSE A BUTTON TYPE     
        [SerializeField] PT_Types platformTriggerType;
        public enum PT_Types                              //BUTTON TYPES
        {
            RealPlatform,
            FakePlatform
        }
        public PT_Types GetPlatformType() => platformTriggerType;      

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [SerializeField] private float duration;
        [SerializeField] private float strength;
        [SerializeField] private int vibrato;
        [SerializeField] private float randomness;
        [SerializeField] private bool isStarted = false;
        [SerializeField] private float roadShowTime=1;
        private Color newColor;
        private Color startColor;
        private Color startEmission;

        [Header("Connected And Controller Objects")]
        [SerializeField] private GameObject[] controllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
        [SerializeField] private bool controllerStatus;           // CHECK ALL Controller BUTTONS


        private void Start()
        {
            myRenderer = GetComponent<Renderer>();
            myCollider = GetComponent<Collider>();

            startColor = myRenderer.material.GetColor("_BaseColor");
            startEmission = myRenderer.material.GetColor("_EmissionColor");
            //myRenderer.material.color.
        }

        private void Update()
        {
           
        }

        public void PressedButton(bool isButtonOn)
        {
            if(!isStarted)
            {
                isStarted = true;
                switch (platformTriggerType)
                {
                    case PT_Types.RealPlatform:
                        newColor = new Color(0.1f, 0.8f, 0, 0.5f);
                        break;
                    case PT_Types.FakePlatform:
                        newColor = new Color(0.8f, 0.1f, 0, 0.5f);
                        break;
                }

                StartCoroutine("ShowRoad", newColor);
            }
            //Show road
        }

        private void OnTriggerEnter(Collider other)
        {  
            switch(platformTriggerType)
            {
                case PT_Types.FakePlatform:
                    if (other.CompareTag("Player"))
                    {
                        StartCoroutine("FakePlatform");
                    }
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
           

            
        }

        IEnumerator FakePlatform()
        {
            transform.DOShakeScale(duration, strength, vibrato, randomness, true);

            yield return new WaitForSeconds(duration);

            transform.DOScale(new Vector3(1,1,1),duration);

            yield return new WaitForSeconds(duration);

            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<Renderer>().enabled = false;

            yield return new WaitForSeconds(2);

            this.gameObject.GetComponent<MeshCollider>().enabled = true;
            this.gameObject.GetComponent<Renderer>().enabled = true;

            yield return null;
        }

        IEnumerator ShowRoad(Color getColor)
        {
            myRenderer.material.SetColor("_BaseColor", getColor);
            myRenderer.material.SetColor("_EmissionColor", getColor);

            yield return new WaitForSeconds(roadShowTime);

            myRenderer.material.SetColor("_BaseColor", startColor);
            myRenderer.material.SetColor("_EmissionColor", startEmission);

            isStarted = false;
            yield return null;
        }


    }
}