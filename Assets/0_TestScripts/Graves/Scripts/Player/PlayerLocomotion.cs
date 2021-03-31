using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerLocomotion : MonoBehaviour
    {
        IPushable pushable;
        Vector3 horVector;
        Vector3 verVector;
        InputHandler inputHandler;
        PlayerAnimatorManager animatorManager;
        PlayerManager playerManager;
        PlayerInteractor playerInteractor;
        public CharacterController cController;

        [Header("Character Interactor")]
        [SerializeField] private GameObject characterInteractor;
        [SerializeField] private float interactionDist=1;

        [Header("Add Force")]
        [SerializeField] private Vector3 impact;

        [Header("Grounded")]
        public float gravity = -9.81f;
        [SerializeField] private float gravityY = -1.5f;
        [SerializeField] private float groundDist = 0.25f;
        [SerializeField] private float groundRad = 0.5f;
        [SerializeField] private LayerMask groundMask;        
        Vector3 gravityDir;
        private bool canSetGravity;

        [Header("Movement Variables")]
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private Vector3 movementVelocity;
        [SerializeField] private Vector3 controllerVelocity;
        [SerializeField] private Vector3 targetVelocity;
        [SerializeField] private float currentSpeed;
        public float walkSpeed = 5;
        public float sprintSpeed = 8;
        public float rotationSpeed = 15;
        public float rollSpeed = 2;
        public bool canRotate=true;
        public bool canMove = true;
        Vector3 targetPos;

        [Header("Falling Variables")]
        [SerializeField] private float fallSpeed=30;
        public float inAirTimer;

        [Header("Push/Pull Variables")]
        public bool canPushPull;
        public float pushSpeed=1.5f;
        public LayerMask pushableMask;
        public RaycastHit pushHit;

        [Header("Climb Variables")]
        public LayerMask climbableWallMask;
        public float climbSpeed = 3f;
        private RaycastHit climbHit;
        private bool climbToFall=false;
        private bool climbToEnd = false;

        [Header("WallRun Variables")]
        public LayerMask walkableWallMask;
        public float wallRunForce;
        public float maxWallRunTime;
        public float maxWallRunSpeed;
        public bool isWallRight;
        public bool isWallLeft;   

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerInteractor = GetComponent<PlayerInteractor>();
            inputHandler = GetComponent<InputHandler>();
            animatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            cController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            canSetGravity = true;
            currentSpeed = walkSpeed;
            verVector = new Vector3(1f,0,1f);
            horVector = new Vector3(1f,0,-1f);
        }      

        public void HandleAllFixed()
        {
            CheckGrounded();
        }
        public void HandleAllMovement()
        {
            HandleSetGravity();
            HandleMovement();
            HandleRollAndSprint();
            HandleFall();          
            HandleImpacting();
            HandlePushPull();
            HandleClimbing();
        }

        #region Check Grounded and Set Gravity
        private void CheckGrounded()
        {
            gravityDir = new Vector3(transform.position.x, transform.position.y - groundDist, transform.position.z);
            playerManager.isGrounded = Physics.CheckCapsule(transform.position, gravityDir, groundRad, groundMask);      
        }
        private void HandleSetGravity()
        {
            if(canSetGravity)
            {
                if (playerManager.isGrounded && movementVelocity.y < 0)
                {
                    movementVelocity.y = gravityY;
                }

                movementVelocity.y += gravity * Time.deltaTime;
                cController.Move(movementVelocity * Time.deltaTime);
            }
        }
        #endregion
        
        private void HandleMovement()
        {      
            if(canMove)
            {
                if (inputHandler.rollFlag || playerManager.inAnim)
                {
                    return;
                }
                controllerVelocity = cController.velocity;

                moveDirection = verVector * inputHandler.verticalInput + horVector * inputHandler.horizontalInput;

                SnapInputs();

                if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
                {
                    currentSpeed = sprintSpeed;
                    playerManager.isSprinting = true;
                }
                else
                {
                    currentSpeed = walkSpeed;
                    playerManager.isSprinting = false;
                }

                cController.Move(moveDirection * currentSpeed * Time.deltaTime);

                animatorManager.UpdateAnimatorValues("moveAmount", inputHandler.moveAmount, playerManager.isSprinting);
                animatorManager.UpdateAnimatorValues("Vertical", inputHandler.verticalInput, false);
                animatorManager.UpdateAnimatorValues("Horizontal", inputHandler.horizontalInput, false);

                if (canRotate)
                {
                    HandleRotation();
                }
            }            

           
        }     

        private void HandleRotation()
        {        
             Vector3 targetDirection = Vector3.zero;
             targetDirection = verVector * inputHandler.verticalInput + horVector * inputHandler.horizontalInput;
             targetDirection.Normalize();
             targetDirection.y = 0;

             if(targetDirection==Vector3.zero)
             {
                 targetDirection = transform.forward;
             }
            
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation,rotationSpeed*Time.deltaTime);

            transform.rotation = playerRotation;
        }

        private void HandleRollAndSprint()
        {
            if(animatorManager.animator.GetBool("inAnim"))
            {
                return;
            }
            
            if(inputHandler.rollFlag)
            {
                moveDirection = verVector * inputHandler.verticalInput + horVector * inputHandler.horizontalInput;

                SnapInputs();

                if (inputHandler.moveAmount>0)
                {
                  animatorManager.PlayTargetAnimation("Rolling", true);
                  moveDirection.y = 0;
                  Quaternion rollRot = Quaternion.LookRotation(moveDirection);
                  transform.rotation = rollRot;
                 //playerManager.isRolling = true;
                }
                else
                {
                       // animatorManager.PlayTargetAnimation("BackStep", true);
                }
            }
        }
     
        private void HandleFall()
        {
            if (canSetGravity)
            {
                if (playerManager.isInAir)
                {
                    cController.Move(moveDirection * fallSpeed * Time.deltaTime);         //kenarda takýlmasýn diye biraz ileri it
                }

                targetPos = transform.position;

                if (playerManager.isGrounded)
                {
                    if (playerManager.isInAir)
                    {
                        if (inAirTimer > 0.5f)
                        {
                            animatorManager.animator.SetFloat("inAirTime", inAirTimer);
                            animatorManager.PlayTargetAnimation("Landing", true);
                            impact = Vector3.zero;                            
                        }
                        else
                        {
                            animatorManager.PlayTargetAnimation("Empty", false);
                        }
                        inAirTimer = 0;
                        playerManager.isInAir = false;
                    }
                    else
                    {
                        if (playerManager.isForced)
                        {
                            impact = Vector3.zero;
                            playerManager.isForced = false;
                        }
                    }
                }
                else
                {
                    animatorManager.animator.SetFloat("inAirTime", inAirTimer);
                    if (!playerManager.isInAir)
                    {
                        if (!playerManager.inAnim)
                        {
                            animatorManager.PlayTargetAnimation("Falling", true);

                        }
                        playerManager.isInAir = true;
                    }
                }

                if (playerManager.inAnim || inputHandler.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
                }
                else
                {
                    transform.position = targetPos;
                }
            }                  
        }

        private void HandlePushPull()
        {
            if (playerManager.isGrounded)
            {
                if(inputHandler.pushPullFlag)
                {   
                    if(playerInteractor.bodyInteractor(Bodyparts.legCenter,pushableMask,0.5f,out pushHit)) //bodyparts fromlegCenter
                    {
                        pushable = pushHit.collider.GetComponent<IPushable>();
                        if (pushable != null && !playerManager.inAnim)
                        {
                            canRotate = false;
                            animatorManager.PlayTargetAnimation("PushPull", true);
                            playerManager.isPushPull = true;
                        }
                    }
                    else
                    {
                        if (playerManager.isPushPull && pushable != null)
                        {
                            animatorManager.PlayTargetAnimation("PushStop", true);
                            playerManager.isPushPull = false;
                            pushable = null;
                            canRotate = true;
                        }
                    }

                    if(playerManager.isPushPull && pushable!=null)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-pushHit.normal), 0.1f); //eger kamera yamuluyorsa bundan dolayý olabilir
                        moveDirection = horVector * inputHandler.horizontalInput + verVector * inputHandler.verticalInput ;
                        SnapInputs();

                        Vector3 checkRot = Vector3.Scale(transform.forward, moveDirection); //strafeler gelirse deðiþecek   

                        if (checkRot.magnitude<0.8)
                        {
                            animatorManager.UpdateAnimatorValues("moveAmount", 0, false);
                            return;
                        }                       

                        float moveAmount = Mathf.Clamp01(Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));

                        if (checkRot.x == -1 || checkRot.z == -1)
                        {
                           moveAmount = -moveAmount;
                        }

                        animatorManager.UpdateAnimatorValues("moveAmount", moveAmount, false);

                        if (moveAmount>0 || moveAmount<0 )
                        {
                            pushable.Pushing(moveDirection * pushSpeed);
                            cController.SimpleMove(moveDirection * pushSpeed);
                        }
                    }
                }
                else
                {
                    if(playerManager.isPushPull && pushable != null)
                    {
                        animatorManager.PlayTargetAnimation("PushStop", true);
                        playerManager.isPushPull = false;
                        pushable = null;
                        canRotate = true;                     
                    }
                }
            }
        } //kodu düzenlee

        private void HandleImpacting()
        {
            if (impact.magnitude > 0.2f)
            {
                cController.Move(impact * Time.deltaTime);             
            } 
        }

        #region CLIMB FUNCTIONS
        private void HandleClimbing()
        {
            if (playerManager.isClimbing)
            {
               if (inputHandler.climbFlag)
               {
                    moveDirection = transform.right * inputHandler.horizontalInput + transform.forward * inputHandler.verticalInput;
                    SnapInputs();
                    float moveA = Mathf.Clamp01(Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y));
                   StartCoroutine(ClimbToFall());
                   // StartCoroutine(ClimbWallJump());
               }

                cController.Move(transform.forward * climbSpeed / 5 * Time.deltaTime); //duvara dogru itiyor***    
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-climbHit.normal), 0.1f);  //duvara dogur bak  //eger kamera yamuluyorsa bundan dolayý olabilir
                moveDirection = transform.right * inputHandler.horizontalInput + new Vector3(0, 1, 0) * inputHandler.verticalInput;
                SnapInputs();

                float moveAmount = Mathf.Clamp01(Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y));

                animatorManager.UpdateAnimatorValues("Vertical", inputHandler.verticalInput, false);
                animatorManager.UpdateAnimatorValues("Horizontal", inputHandler.horizontalInput, false);

                if (moveAmount > 0)
                {
                    playerInteractor.bodyInteractor(Bodyparts.hipsCenter, climbableWallMask, 1, out climbHit);
                    cController.Move(moveDirection * climbSpeed * Time.deltaTime);
                }

                if (!playerInteractor.bodyInteractor(Bodyparts.headCenter, climbableWallMask, 1) && !climbToFall)
                {
                    climbToEnd = true;
                    StartCoroutine(ClimbEnd());
                }
                else if (playerInteractor.bodyInteractor(Bodyparts.headCenter, climbableWallMask, 1) && (!playerInteractor.bodyInteractor(Bodyparts.hipsRight,climbableWallMask,1) || !playerInteractor.bodyInteractor(Bodyparts.hipsLeft, climbableWallMask, 1)) && !climbToEnd)
                {
                    climbToFall = true;
                   StartCoroutine(ClimbToFall());
                }       
            }
            else if (!playerManager.isClimbing)
            {
                if (inputHandler.climbFlag && !playerManager.isInAir)
                {
                    if (playerInteractor.bodyInteractor(Bodyparts.headCenter,climbableWallMask,0.5f) && playerInteractor.bodyInteractor(Bodyparts.headRight,climbableWallMask, 0.5f) && playerInteractor.bodyInteractor(Bodyparts.headLeft, climbableWallMask, 0.5f))
                    {
                        if (!playerManager.inAnim)
                        {
                            playerManager.isClimbing = true;
                            canRotate = false;
                            canMove = false;
                            canSetGravity = false;
                            animatorManager.PlayTargetAnimation("Climbing", true);
                        }
                    }
                }
            }
        }

        IEnumerator ClimbEnd()
        {
            animatorManager.PlayTargetAnimation("ClimbEnd", true);            
            float elapsedTime = 0;
            playerManager.isClimbing = false;
            playerManager.isGrounded = true;
            while (elapsedTime<=0.65f)
            {
                elapsedTime += Time.deltaTime;
                cController.Move(transform.up * 0.75f * Time.deltaTime);
                cController.Move(transform.forward * 0.75f * Time.deltaTime);
                yield return null;
            }
            canSetGravity = true;
            canRotate = true;
            canMove = true;
            climbToEnd = false;
            yield break;
        }

        void SetNewPos(Vector3 newDir, float force)
        {
           // cController.Move(newDir * force * Time.deltaTime);
        }


        IEnumerator ClimbToFall()
        {
            animatorManager.UpdateAnimatorValues("Vertical", 0, false);
            animatorManager.UpdateAnimatorValues("Horizontal", 0, false);
            inAirTimer = 0.4f;

            if (!playerInteractor.bodyInteractor(Bodyparts.headRight, climbableWallMask, 1))
            {
                cController.Move(transform.right * 5 * Time.deltaTime);
            }
            else if (!playerInteractor.bodyInteractor(Bodyparts.headLeft, climbableWallMask, 1))
            {
                cController.Move(-transform.right * 5 * Time.deltaTime);
            }

            //***************** movelar yerine impacti kullan daha mantýklý sürekli sýfýrlamakla uðraþmazsýn****************

            cController.Move(-transform.forward * 10 * Time.deltaTime);
            canSetGravity = true; //coroutine gelebilir buraya****
            playerManager.isClimbing = false;
            playerManager.isGrounded = false;
            playerManager.isInAir = true;
            canRotate = true;
            canMove = true;

            animatorManager.PlayTargetAnimation("Falling", true);
            climbToFall = false;
            yield break;
        }

        IEnumerator ClimbWallJump()
        {
            animatorManager.PlayTargetAnimation("WallJumping", true);           
            playerManager.isClimbing = false;
            playerManager.isGrounded = false;
            playerManager.isInAir = true;
            yield return new WaitForSeconds(1.05f);

            if (playerInteractor.bodyInteractor(Bodyparts.hipsCenter,climbableWallMask,1))
            {
                Debug.Log("sex");
                //playerManager.isClimbing = true;
            }
            else
            {
               
              //  StartCoroutine(ClimbToFall());
            }
      
            yield break;
        }

        #endregion

        #region Functions
        public void HandleAddImpact(Vector3 forceDir,float addForce)
        {
            forceDir.Normalize();
            if(forceDir.y<0)
            {
                forceDir.y = -forceDir.y;
            }
            impact += forceDir.normalized * addForce;
            StartCoroutine(WaitForSecs());
        }

        private void SnapInputs()
        {
            #region SnapDirection
            if (moveDirection.x >= 1)
            {
                moveDirection.x = 1;
            }
            else if (moveDirection.x <= -1)
            {
                moveDirection.x = -1;
            }

            if (moveDirection.y >= 1)
            {
                moveDirection.y = 1;
            }
            else if (moveDirection.y <= -1)
            {
                moveDirection.y = -1;
            }

            if (moveDirection.z >= 1)
            {
                moveDirection.z = 1;
            }
            else if (moveDirection.z <= -1)
            {
                moveDirection.z = -1;
            }
            #endregion

            var a = moveDirection.x * moveDirection.z;

            if (a >= 1 || a <= -1)
            {
                moveDirection.x *= 0.707f;
                moveDirection.z *= 0.707f;
            }

        }

        IEnumerator WaitForSecs()
        {
            yield return new WaitForSeconds(0.2f);
            playerManager.isForced = true;
        }

        #endregion
    }
}