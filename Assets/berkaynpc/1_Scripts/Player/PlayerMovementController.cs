using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace project_WAST
{
    public class PlayerMovementController : MonoBehaviour
    {
        CharacterController chController;
        InputManager inputManager; //input scriptinden deðerleri almak için tanýmlandýk      
        PlayerAnimationController playerAnimController;

        [Header("Movement")]
        public bool canMove = true;
        [Range(0, 20)]
        public float speed;
        public float moveControl;
        public Vector3 velocity;
        [SerializeField] private Vector3 controllerVelocity;           
        [SerializeField] private float velocity_Y = -1.5f;
        public bool isJumping = false;
        private Vector3 moveDirection;
        private float HorizontalMove;
        private float VerticalMove;

        [Header("Ground")] //bu gidebilir yerine controllerýn kendisi kullanýlabilir***
        public Transform groundCheck;
        public LayerMask groundMask;
        public bool isGrounded;
        public float groundDistance = 0.4f;
        public float gravity = -9.81f;               
  
        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            playerAnimController = GetComponent<PlayerAnimationController>();
            chController = GetComponent<CharacterController>();
        }
      
        private void Update()
        {   
            controllerVelocity = chController.velocity; 
            HorizontalMove = inputManager.horizontalInput;
            VerticalMove = inputManager.verticalInput;

            moveControl = new Vector2(VerticalMove, HorizontalMove).sqrMagnitude;          

            if (moveControl <= 0f)
            {

            }
            else if(moveControl > 0 && canMove && !isJumping)
            {              
                Movement();
            }            

             velocity.y += gravity * Time.deltaTime;
             chController.Move(velocity * Time.deltaTime);
        }

        void FixedUpdate()
        {
           isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

           if (isGrounded && velocity.y < 0)
           {
                canMove = true;
                velocity.y = velocity_Y;
                velocity.x = 0;
                velocity.z = 0;
                isJumping = false;
           }
        }
        
        private void Movement()
        {  
            moveDirection = transform.right * HorizontalMove + transform.forward * VerticalMove;
            chController.Move(moveDirection * speed * Time.deltaTime);
        }

        private void Rotation()
        {

        }       
        
    }
}