using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class ObjectInteractMark : MonoBehaviour
    {
       private Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void ShowMe(bool canShow)
        {
            animator.SetBool("canShowUI", canShow);
        }
    }
}