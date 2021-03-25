using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    public abstract class CharacterStateManager : StateManager
    {
        [Header("References")]
        public Animator anim;
        public new Rigidbody rigidbody;

        [Header("Controller Values")]
        public float vertical;
        public float horizontal;
        public bool lockOn;
        public float delta;

        public override void Init()
        {
            anim = GetComponentInChildren<Animator>();
            rigidbody = GetComponentInChildren<Rigidbody>();
        }

        public void PlayTargetAnimation(string targetAnim) 
        {
            anim.CrossFade(targetAnim, 0.2f);
        }

    }
}