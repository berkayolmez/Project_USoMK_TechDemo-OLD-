using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace project_WAST
{
    public class IPickable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string myPickableText;
        public Sprite myPickableIcon;

        //public Image pickableIcon; //daha sonra item yanýna gelince çýkacak görsel ve animasyon için 

        public virtual void NearByObject()
        {
            //call when player near by the object
        }

        public virtual void PickInteract(PlayerManager playerManager)
        {
            //call when player interact=?????
        }

    }
}