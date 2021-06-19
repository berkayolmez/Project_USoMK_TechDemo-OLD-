using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    /// <summary>
    /// This class is for object that have logic gates and triggered by controller objects.
    /// </summary>
    public abstract class ObjectBase : ObserverBase 
    {
        [Header("Logic Gate Type")]
        public MyFunctions.LogicGateType myLogicGateType;  //Object's Logic Gate Type
        protected override void Start()
        {
            base.Start();
        }
    }
}