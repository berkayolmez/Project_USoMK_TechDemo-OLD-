using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace project_usomk
{
    public class UI_Interact : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        Animator animator;
        [SerializeField] private GameObject UIPanel;
        public string currentAnim;
        public bool hideWithMouse;

        private void Start()
        {
                animator = GetComponent<Animator>();           
        }

        public void OnPointerEnter(PointerEventData eventData)      //When the cursor is over UI object.
        {
            if(animator!=null)
            {
                animator.CrossFade(currentAnim, 0.2f);
                animator.SetBool("isMouseOn", true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)       //When the cursor leaves UI object.
        {
            if(animator!=null)
            {
                animator.SetBool("isMouseOn", false);
            }

            if(UIPanel!=null && hideWithMouse)
            {
               UIPanel.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)       //When the cursor is clicked.
        {
            if(UIPanel!=null)
            {
                animator.SetBool("isMouseOn", false);
                UIPanel.SetActive(true);
            }
        }
    }
}