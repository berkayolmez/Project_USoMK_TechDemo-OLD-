using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace project_WAST
{
    public abstract class StateBase : MonoBehaviour
    {
        public abstract StateBase RunCurrentState(); 
       


    }
}