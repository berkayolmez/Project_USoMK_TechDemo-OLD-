using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace project_usomk
{
    public class IPickable : MonoBehaviour
    {
        public float radius = 0.6f;     //pickable area radius
        public string myPickableText;       //item UI text
        public Sprite myPickableIcon;       //item UI icon

        //public Image pickableIcon; //For the UI visual that will appear when you come near the pickable items.
        //call when player near by the object
        public virtual void NearByObject()
        {
          
        }

        //call when player interact
        public virtual void PickInteract(PlayerManager playerManager)
        {
            
        }

    }
}