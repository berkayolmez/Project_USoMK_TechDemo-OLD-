using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public interface IPooled //This interface will be used to tag all spawnable objects in the object pool.
    {
     string myTag { get; set; }        
    }
}