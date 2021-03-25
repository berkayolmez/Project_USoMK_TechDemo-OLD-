using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class GlobalObjetcs : MonoBehaviour, IHaveButton, IHaveStatus
    {
        private Light thisLight;
        private MyFunctions myFunctions = new MyFunctions();  //BU DEGISEBILIR FARKLI YOL BULUNURSA********

        [Header("My Status (True/False)")]                    // THIS BUTTON'S STATUS
        [SerializeField] private bool lightStatus = true;
        public bool myStatus => lightStatus;            // THIS BUTTON'S STATUS TO IINTERACTABLE INTERFACE

        [Header("Select a Object Type")]                               //CHOOSE A Object TYPE     
       public LightTypes lightType;
        public enum LightTypes                               
        {
            SimpleLight, 
            AdjustableLight,
            //BlinkingLight,        //eklenecek
            // TrackerLight,         //eklenecek
            // EscortLight,          //eklenecek
            // ColorfulLight,        //eklenecek// düzelnlenecek ayrý ayrý renk deðiþimi yapýlamýyor çözüm bul
        }
        public LightTypes GetLightType() => lightType;


        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Button Settings")]                              //BUTTON VARIABLES        
       // [SerializeField] private float lightRange;
        [SerializeField] private float lightIntensity;
        [SerializeField] private float maxLightIntensity;
        [SerializeField] private Color lightColor;
        [SerializeField] private float loaderSpeed; //get from controller button
        [SerializeField] private bool isLoading;  //get from myFunction script

        [Header("Controller Objects")]
        public GameObject[] upControllerObjs;     // ALL CONNECTED BUTTONS TO THIS BUTTON (THIS BUTTON CAN CONTROL BY ANOTHER BUTTON OR BUTTONS)
       [HideInInspector] public GameObject[] downControllerObjs;
        public bool controllerStatusUp;           // CHECK ALL Controller BUTTONS
        [HideInInspector] public bool controllerStatusDown;

        private void Awake()
        {          
            switch (lightType)                                  //OBJECT INSTANTIATE SETTINGS
            {
                case LightTypes.SimpleLight:                      //THIS TWO TYPE COULD BE OFF WHEN START
                case LightTypes.AdjustableLight:              
                    thisLight = GetComponent<Light>();                  
                    break;
            }
        }

        private void Start()
        {
            thisLight.intensity = lightIntensity;
            thisLight.color = lightColor;
        }

        private void Update()
        {
            if (isLoading)
            {
                switch (lightType)
                {
                    case LightTypes.AdjustableLight:                      
                        lightIntensity = myFunctions.Loader(loaderSpeed, maxLightIntensity);                       

                        if (lightIntensity <= 0)
                        {
                            lightIntensity = 0;                           
                        }

                        if(lightIntensity>=maxLightIntensity)
                        {
                            isLoading = false;
                        }

                        thisLight.intensity = lightIntensity;
                        break;
                }
            }
        }

        public void PressedButton(bool isButtonOn)
        {
            switch (lightType)
            {
                case LightTypes.SimpleLight:

                    controllerStatusUp = myFunctions.CheckControllerObjects(upControllerObjs, myLogicGateType);

                    switch (myLogicGateType) //daha iyi bulunursa silinir
                    {
                        case MyFunctions.LogicGateType.ORGate:
                            isButtonOn = controllerStatusUp;
                            break;
                    }

                    lightStatus = isButtonOn;

                    if (isButtonOn)
                    {
                        thisLight.intensity = maxLightIntensity;
                    }
                    else
                    {
                        thisLight.intensity = 0;
                    }
                    break;

                case LightTypes.AdjustableLight:

                    controllerStatusUp = myFunctions.CheckControllerObjects(upControllerObjs, myLogicGateType);
                    controllerStatusDown = myFunctions.CheckControllerObjects(downControllerObjs, myLogicGateType);

                    if(controllerStatusUp && isButtonOn)
                    {
                        loaderSpeed = Mathf.Abs(loaderSpeed);
                    }
                    else if(controllerStatusDown)
                    {
                        loaderSpeed = -loaderSpeed;
                    }

                    isLoading = isButtonOn;
                    break;
            }
            //Dýþarýdan biþi olursa resetlemek için reset fonksiyonu yazýlabilir               
        }
    }
    
}