using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class AddToPool : MonoBehaviour,IPooled
    {
        [SerializeField] private string objectTag; 
        string IPooled.myTag { get => objectTag; set => objectTag = value; }
    }
}