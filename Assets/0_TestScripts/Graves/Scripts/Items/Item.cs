using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace project_WAST
{
    public class Item : ScriptableObject
    {
        [Header("Item Info")]
        public Sprite itemIcon;
        public string itemName; 
    }
}