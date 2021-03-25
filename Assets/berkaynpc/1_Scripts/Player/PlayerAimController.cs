using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace project_WAST
{
    public class PlayerAimController : MonoBehaviour
    {
        InputHandler inputHandler; //input scriptinden deðerleri almak için tanýmlandýk        
        PlayerAnimationController playerAnimController;

        [Header("Objects")]
       // [SerializeField] private Transform aimHolder;

        [Header("Bools")]
        [SerializeField] private bool gamepad = false;
        [SerializeField] private  bool mouse = false;
        [SerializeField] private bool isPistol = false;

        [Header("Vectors")]
        public Vector3 mousePos;
        public Vector2 rotationInput;
        public Vector2 lastRotInput;
        public Vector3 lastRotDirection;
        public Vector3 playerXYZ;        

        [Header("Values")]
        public float animAngle;
        public float aimAngle;
        public float angleRot;
        public float angleFix;
        public float lastRotH;
        public float lastRotHAim;
        public float lastRotV;
        public float lastRotVAim;
        public float HorizontalRot;
        public float HorizontalRotTest;
        public float VerticalRot;
        public float VerticalRotTest;

        private void Awake()
        {
            playerAnimController = GetComponent<PlayerAnimationController>();
            inputHandler = GetComponent<InputHandler>();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isPistol = !isPistol;
                playerAnimController.SetPistol(isPistol);
            }
        }

        void FixedUpdate()
        {/*  **************************bunu ac
            if (inputHandler.rightStick.x > 0.01f || inputHandler.rightStick.x < -0.01f || inputHandler.rightStick.y > 0.01f || inputHandler.rightStick.y < -0.01f)
            {
                gamepad = true;
                mouse = false;
            }
            else if (Mouse.current.delta.x.ReadValue() > 0.01f || Mouse.current.delta.x.ReadValue() < -0.01f || Mouse.current.delta.y.ReadValue() > 0.01f || Mouse.current.delta.y.ReadValue() < -0.01f)
            {
                mouse = true;
                gamepad = false;
            }
           
            if (gamepad && !mouse)
            {
                  //  GamepadAimFix();
            }
            else if (!gamepad && mouse)
            {
               MouseAimTestFix();
            }    */
        }

        void MouseAimTestFix()
        {
            //mouse konumu alýndý
            mousePos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            mousePos -= new Vector3(0.5f, 0.5f, 0.0f) * 0.96f;

            //açý hesabý
            animAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            //aim açýsý için
            aimAngle = animAngle * 0.99f;

            // sýfýrdan küçükse 360 ekleyerek + eksene döndürme
            if (animAngle < 0)
            {
                animAngle += 360;
            }

            ///// animasyon için açýyý düzeltme
            if (animAngle < 323.5f && animAngle > 292.5f)
            {
                animAngle = 315f;
            }
            else if (animAngle < 292.5f && animAngle > 247.5f)
            {
                animAngle = 270f;
            }
            else if (animAngle < 247.5f && animAngle > 202.5f)
            {
                animAngle = 225f;
            }
            else if (animAngle < 202.5f && animAngle > 157.5f)
            {
                animAngle = 180f;
            }
            else if (animAngle < 157.5f && animAngle > 112.5f)
            {
                animAngle = 135f;
            }
            else if (animAngle < 112.5f && animAngle > 67.5)
            {
                animAngle = 90f;
            }
            else if (animAngle < 67.5 && animAngle > 22.5f)
            {
                animAngle = 45f;
            }
            else if (animAngle < 22.5f || animAngle > 337.5f)
            {
                animAngle = 0f;
            }

            //animasyon için açý giriþi
            playerAnimController.SetAnimAngle(animAngle);

            //aim açýsý kübe uygulandý
            //aimHolder.localRotation = Quaternion.AngleAxis(aimAngle, Vector3.down);
        }


        void GamepadAimFix()
        {
            //*****  playerXYZ = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, playerObj.transform.position.z);

            //rotationInput = inputHandler.rotationInput;  //Aim ve ateþ için 
            HorizontalRot = rotationInput.x;
            VerticalRot = rotationInput.y;

            //lastRotInput = inputHandler.rightStickLastRot; //Animasyon için **************************bunu ac
            HorizontalRotTest = lastRotInput.x;
            VerticalRotTest = lastRotInput.y;


            if (HorizontalRotTest > 0.01f || HorizontalRotTest < -0.01f || VerticalRotTest > 0.01f || VerticalRotTest < -0.01f)
            {
                //Animasyon için son hesap

                //***** animator.SetFloat("lastRotH", HorizontalRotTest);
                //*****animator.SetFloat("lastRotV", VerticalRotTest);
                lastRotHAim = HorizontalRotTest;
                lastRotVAim = VerticalRotTest;
            }



            if (HorizontalRot > 0.01f || HorizontalRot < -0.01f || VerticalRot > 0.01f || VerticalRot < -0.01f)
            {
                //Aim için son 

                lastRotH = HorizontalRot;
                lastRotV = VerticalRot;
            }

            //Gamepad durduktan sonraki son yön hesabý
            lastRotDirection = new Vector3(lastRotH, 0f, lastRotV);



            angleRot = Mathf.Atan2(lastRotVAim, lastRotHAim) * Mathf.Rad2Deg; // animasyon için açý hesap
            //float angleRot = Mathf.Atan2(lastRotV, lastRotH) * Mathf.Rad2Deg;

            float aimlast = Mathf.Atan2(lastRotV, lastRotH) * Mathf.Rad2Deg;

            if (angleRot < 0)
            {
                angleRot = angleRot + 360;
            }

            ///// Yürüme kontrol /////

            if (angleRot / angleFix <= 1.1f && angleRot / angleFix >= 0.9f)
            {
                //*****  animator.SetFloat("Speed", 1f);
            }
            else
            {
                //***** animator.SetFloat("Speed", -1f);
            }

            //***** animator.SetFloat("angleRot", angleRot, 0.0f, Time.deltaTime * 2f);

            //aimHolder.localRotation = Quaternion.AngleAxis(aimlast, Vector3.down);

            Debug.DrawRay(playerXYZ, lastRotDirection * 50, Color.blue);
        }

    }

}
