using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class MyFunctions
    {
        #region Interfaces
        public IInteractable interactable;
        public IHold iHold;
        private IHaveButton iHaveButton;
        private IHaveStatus iHaveStatus;
        private IHaveSignal iHaveSignal;
        #endregion

        #region Some Variables
        public float loadingValue = 0;      //For loader method
        public bool isLoadingDone=false;        //For loader method
        #endregion

        public enum LogicGateType
        {
            DontHaveGate,
            ORGate,
            NORGate,
            XORGate,
            XNORGate, 
            ANDGate,
            NANDGate,
            Not
        }

        /// <summary>
        /// This method checks the status of controllers considering the selected logic gate.
        /// </summary>
        // Logic gate implementation. 
        public virtual bool CheckControllerStatus(List<GameObject> controllerObjetcs, LogicGateType getGateType) 
        {
            if (controllerObjetcs.Count > 0)
            {
                switch (getGateType)
                {
                    case LogicGateType.Not:
                    case LogicGateType.DontHaveGate:
                        return true;

                    case LogicGateType.ORGate:

                        foreach (var buttons in controllerObjetcs)
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && iHaveStatus.myStatus)
                            {
                                return true;
                            }
                        }
                        return false;

                    case LogicGateType.ANDGate:

                        foreach (var buttons in controllerObjetcs)
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && !iHaveStatus.myStatus)
                            {
                                return false;
                            }
                        }
                        return true;

                    case LogicGateType.NORGate:

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && iHaveStatus.myStatus)
                            {
                                return false;
                            }
                        }
                        return true;

                    case LogicGateType.NANDGate:

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && !iHaveStatus.myStatus)
                            {
                                return true;
                            }
                        }
                        return false;

                    case LogicGateType.XORGate: //bunu incele

                        bool xOR = false;

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null)
                            {
                                if (xOR != iHaveStatus.myStatus)
                                {
                                    xOR = true;
                                }
                                else if (xOR = iHaveStatus.myStatus)
                                {
                                    xOR = false;
                                }
                            }
                        }

                        if (xOR)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case LogicGateType.XNORGate: //bunu incele

                        bool xNOR = false;

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null)
                            {
                                if (xNOR != iHaveStatus.myStatus)
                                {
                                    xNOR = true;
                                }
                                else if (xNOR = iHaveStatus.myStatus)
                                {
                                    xNOR = false;
                                }
                            }
                        }

                        if (xNOR)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                }

                return false;

            }
            else
            {
                switch (getGateType) //bu deðiþecek controller<0 vs
                {
                    case LogicGateType.DontHaveGate:
                        return true;
                }

                return false;
            }
        }

        public virtual float Loader(float timerSpeed,float maxValue)
        {
            loadingValue += timerSpeed * Time.deltaTime;
                     
            if(loadingValue>= maxValue)
            {              
                isLoadingDone = true;
                return maxValue;
            }
            else
            {
                isLoadingDone = false;
                return loadingValue;
            }   
        }

        /// <summary>
        /// Add a force to the rigidbody object
        /// </summary>
        public virtual void AddForceToObject(GameObject getObject,Transform forcePos,float forceValue)
        {
            Rigidbody newObjRigid = getObject.GetComponent<Rigidbody>();
            if (newObjRigid != null)
            {
                newObjRigid.velocity = Vector3.zero;
                newObjRigid.AddForce(-forcePos.up * forceValue, ForceMode.Impulse);
            }
        }

        #region Old methods //this methods will be remove after refactory done
        public virtual bool CheckControllerObjects(GameObject[] controllerObjetcs, LogicGateType getGateType)
        {
            if (controllerObjetcs.Length > 0)
            {
                switch (getGateType)
                {
                    case LogicGateType.Not:
                    case LogicGateType.DontHaveGate:
                        return true;

                    case LogicGateType.ORGate:

                        foreach (var buttons in controllerObjetcs)
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && iHaveStatus.myStatus)
                            {
                                return true;
                            }
                        }
                        return false;

                    case LogicGateType.ANDGate:

                        foreach (var buttons in controllerObjetcs)
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && !iHaveStatus.myStatus)
                            {
                                return false;
                            }
                        }
                        return true;

                    case LogicGateType.NORGate:

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && iHaveStatus.myStatus)
                            {
                                return false;
                            }
                        }
                        return true;

                    case LogicGateType.NANDGate:

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null && !iHaveStatus.myStatus)
                            {
                                return true;
                            }
                        }
                        return false;

                    case LogicGateType.XORGate: //bunu incele

                        bool xOR = false;

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null)
                            {
                                if (xOR != iHaveStatus.myStatus)
                                {
                                    xOR = true;
                                }
                                else if (xOR = iHaveStatus.myStatus)
                                {
                                    xOR = false;
                                }
                            }
                        }

                        if (xOR)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case LogicGateType.XNORGate: //bunu incele

                        bool xNOR = false;

                        foreach (var buttons in controllerObjetcs) //bu rpc ye gidebilir ????????
                        {
                            iHaveStatus = buttons.GetComponent<IHaveStatus>();

                            if (iHaveStatus != null)
                            {
                                if (xNOR != iHaveStatus.myStatus)
                                {
                                    xNOR = true;
                                }
                                else if (xNOR = iHaveStatus.myStatus)
                                {
                                    xNOR = false;
                                }
                            }
                        }

                        if (xNOR)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }

                }

                return false;

            }
            else
            {
                switch (getGateType) //bu deðiþecek controller<0 vs
                {
                    case LogicGateType.DontHaveGate:
                        return true;
                }

                return false;
            }
        } //useless
        public virtual void SetMyConnectedObjects(GameObject[] connectedObjects, bool getBool)
        {
            foreach (var myObject in connectedObjects)
            {
                iHaveButton = myObject.GetComponent<IHaveButton>();

                if (iHaveButton != null)
                {
                    iHaveButton.PressedButton(getBool);
                }
            }
        }
        public virtual void SetMyConnectedObjects(GameObject[] connectedObjects, float mySignalValue, float myMaxSignalValue)
        {
            foreach (var myObject in connectedObjects)
            {
                iHaveSignal = myObject.GetComponent<IHaveSignal>();

                if (iHaveSignal != null)
                {
                    iHaveSignal.GetSignal(mySignalValue, myMaxSignalValue);
                }
            }
        }

        #endregion 

        #region check this later
        /* //later****
        public virtual void GetKey(RequirementTypes.RequirementType reqType,List<RequirementTypes.RequirementType> getList)
        {
            //  Debug.Log("added Key: " + reqType);
            if (!ContainsKey(reqType,getList))
            {
                AddKey(reqType);
                //requirementList.Add(reqType);
                KeyChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual List<RequirementTypes.RequirementType> AddKey(RequirementTypes.RequirementType reqType)
        {
            return requirementList;
        }

        public virtual void DeletKey(RequirementTypes.RequirementType reqType)
        {
            requirementList.Remove(reqType);
            KeyChange?.Invoke(this, EventArgs.Empty);
        }

        public virtual bool ContainsKey(RequirementTypes.RequirementType reqType,List<RequirementTypes.RequirementType> getList)
        {
            return getList.Contains(reqType);
        }
        */
        #endregion

    }
}