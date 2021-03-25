using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sharp
{
    public class PlayerStateManager : CharacterStateManager
    {       
        [Header("Inputs")]
        public float mouseX;
        public float mouseY;
        public float moveAmount;
        public Vector3 moveDirection;
        public Vector3 rotateDirection;

        [Header("States")]
        public bool isGrounded;

        [Header("Movemen Stats")]
        [Range(1,50)]
        public float rotSpeed=5;
        public float frontRayOffset = 0.5f;
        public float movementSpeed = 2;
        public float adaptSpeed = 10;
        public LayerMask groundLayer;

        [HideInInspector]
        public string locomotionId = "locomotion";
        [HideInInspector]
        public string attacStateId = "attacState";

        public override void Init()
        {
            base.Init();

            State locomotion = new State(
              new List<StateAction>() //fixedupdate
              {               
                new MovePlayerCharacter(this),
              },
              new List<StateAction>() //update
              {
                 
              },
              new List<StateAction>() //lateupdate
              {
            
              }
              );
            
            State attacState = new State(
              new List<StateAction>() //fixedupdate
              {

              },
              new List<StateAction>() //update
              {

              },
              new List<StateAction>() //lateupdate
              {

              }
              );

            RegisterState("locomotion", locomotion);
            RegisterState("atackState", attacState);

            ChangeState("locomotion");
        }

        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            base.FixedTick();
        }

        private void Update()
        {
            delta = Time.deltaTime;
            base.Tick();
        }

        private void LateUpdate()
        {
            base.LateTick();
        }
    }
}