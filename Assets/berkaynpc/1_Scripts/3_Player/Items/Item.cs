using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace project_usomk
{
    /// <summary>
    /// Item base class as scriptable object
    /// </summary>
    public class Item : ScriptableObject
    {
        [Header("Item Info")]
        public Sprite itemIcon;
        public string itemName; 
    }
}