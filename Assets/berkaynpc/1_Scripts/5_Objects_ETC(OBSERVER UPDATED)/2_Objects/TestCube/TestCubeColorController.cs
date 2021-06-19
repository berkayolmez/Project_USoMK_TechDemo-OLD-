using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class TestCubeColorController : ObjectBase, IHaveSignal
    {
        #region cubeType

        [Header("Object Type")]
        [SerializeField] CubeType cubeType;
        public enum CubeType
        {
            OnOffCube,
            MultipleColorCube,
            SignalCube,
        }
        public CubeType GetCubeType()
        {
            return cubeType;
        }

        #endregion    

        [Header("Object Variables")]
        private Renderer _thisRenderer;
        private MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            _thisRenderer = GetComponent<Renderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        protected override void Start()
        {
            base.Start();
            MyGameEvents.current.onSetSignal += OnSettingSignal;
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
                myControllerStatus = myFunctions.CheckControllerStatus(myControllerObjList, myLogicGateType);

                if (myControllerStatus)
                {
                    switch (cubeType)
                    {
                        case CubeType.OnOffCube:

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
                                _thisRenderer.GetPropertyBlock(_propBlock);
                                _propBlock.SetColor("_BaseColor", Color.green);
                                _propBlock.SetColor("_EmissionColor", Color.green);
                                _thisRenderer.SetPropertyBlock(_propBlock);
                            }
                            else
                            {
                                _thisRenderer.GetPropertyBlock(_propBlock);
                                _propBlock.SetColor("_BaseColor", Color.red);
                                _propBlock.SetColor("_EmissionColor", Color.red);
                                _thisRenderer.SetPropertyBlock(_propBlock);
                            }
                            break;

                        case CubeType.MultipleColorCube:
                            _thisRenderer.GetPropertyBlock(_propBlock);
                            _propBlock.SetColor("_BaseColor", GetRandomColor());
                            _propBlock.SetColor("_EmissionColor", _propBlock.GetColor("_BaseColor"));
                            _thisRenderer.SetPropertyBlock(_propBlock);
                            break;
                    }
                }
                else if (!myControllerStatus)
                {
                    myCurrentStatus = false;
                    myControllerStatus = false;
                    _thisRenderer.GetPropertyBlock(_propBlock);
                    _propBlock.SetColor("_BaseColor", Color.red);
                    _propBlock.SetColor("_EmissionColor", Color.red);
                    _thisRenderer.SetPropertyBlock(_propBlock);
                }
            }
        }

        private Color GetRandomColor()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        #region For Signal Cube

        private void OnSettingSignal(string getID, float getMySignal, float getMaxSignal)
        {
            if(getID == this.myID)
            {
                GetSignal(getMySignal, getMaxSignal);               
            }
        }

        public void GetSignal(float getSignal, float maxSignal)
        {
            float converter = getSignal / (2 * maxSignal); 

            switch (cubeType)
            {
                case CubeType.SignalCube:
                    Color newColor = new Color(0.5f + converter, 0.3f + converter, 0.5f + converter);
                    _thisRenderer.GetPropertyBlock(_propBlock);
                    _propBlock.SetColor("_BaseColor", newColor);
                    _propBlock.SetColor("_EmissionColor", newColor);
                    _thisRenderer.SetPropertyBlock(_propBlock);
                    break;
            }
        }
        #endregion

        protected override void OnDestroy() 
        {
            MyGameEvents.current.onSetSignal -= OnSettingSignal;
        }
    }
}