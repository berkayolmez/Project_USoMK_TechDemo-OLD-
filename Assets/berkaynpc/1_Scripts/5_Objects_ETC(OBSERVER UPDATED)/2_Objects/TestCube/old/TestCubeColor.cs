using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class TestCubeColor : MonoBehaviour, IHaveButton, IHaveStatus , IHaveSignal
    {
        private MyFunctions myFunctions = new MyFunctions();

        [Header("My Status (True/False)")]
        [SerializeField] private bool testCubeStatus;

        public bool myStatus => testCubeStatus;

        [Header("Object Type")]
        [SerializeField] CubeType cubeType;
        public enum CubeType
        {
            simple,
            morethanone,
            SignalCube,
        }
        public CubeType GetCubeType()
        {
            return cubeType;
        }       

        [Header("Gate Type")]
        [SerializeField] private MyFunctions.LogicGateType myLogicGateType;

        [Header("Object Variables")]
        public Renderer thisRenderer;

        [SerializeField] private GameObject[] connectedGameObjs;
        [SerializeField] private GameObject[] controllerObjs;
        [SerializeField] private bool controllerStatus;       

        private void Awake()
        {
            thisRenderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            PressedButton(false);
        }

        public void PressedButton(bool isButtonOn)
        {        
            controllerStatus = myFunctions.CheckControllerObjects(controllerObjs, myLogicGateType);       

            if(controllerStatus)
            {               
                switch (cubeType)
                {
                    case CubeType.simple:

                        testCubeStatus = controllerStatus;

                        switch (myLogicGateType) //daha iyi bulunursa silinir
                        {
                            case MyFunctions.LogicGateType.DontHaveGate:
                                testCubeStatus= isButtonOn;
                                break;
                            case MyFunctions.LogicGateType.Not:
                                testCubeStatus = !isButtonOn;
                                break;
                        }
                       

                        if (testCubeStatus)
                        {
                            thisRenderer.material.SetColor("_BaseColor", Color.green);
                            thisRenderer.material.SetColor("_EmissionColor", Color.green);
                        }
                        else
                        {
                            thisRenderer.material.SetColor("_BaseColor", Color.red);
                            thisRenderer.material.SetColor("_EmissionColor", Color.red);
                        }

                        break;


                    case CubeType.morethanone:                       
                            Color newColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                            thisRenderer.material.SetColor("_BaseColor", newColor);
                            thisRenderer.material.SetColor("_EmissionColor", newColor);  
                        break;
                }
            }
            else if(!controllerStatus)
            {               
               testCubeStatus = false;
               controllerStatus = false;
               thisRenderer.material.SetColor("_BaseColor", Color.red);
               thisRenderer.material.SetColor("_EmissionColor", Color.red);   
            }
            
        }

        public void GetSignal(float getSignal,float maxSignal)
        {
            float converter = getSignal / (2*maxSignal);  //0-0.5 arasý tutuyor.

            switch (cubeType)
            {
                case CubeType.SignalCube:

                    Color newColor = new Color(0.5f + converter, 0.3f + converter, 0.5f + converter);

                    thisRenderer.material.SetColor("_BaseColor", newColor);
                    thisRenderer.material.SetColor("_EmissionColor", newColor);

                    break;
            }
        }
    }

}