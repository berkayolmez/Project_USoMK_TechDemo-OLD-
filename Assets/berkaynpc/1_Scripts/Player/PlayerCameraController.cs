using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

namespace project_WAST
{
    public class PlayerCameraController : MonoBehaviour
    {
        public static PlayerCameraController Instance { get; private set; }

        private CinemachineVirtualCamera virtualCam;
        private PlayerManager playerManager;
        public GameObject mainCam;
        public GameObject climbCam;

        private float startingIntensity;
        private float shakeTimerTotal;
        private float shakeTimer;

        private void Awake()
        {
            Instance = this;
            virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
            playerManager = GetComponentInParent<PlayerManager>();
        }

        public void ShakeCamera(float intensity,float time)
        {
            CinemachineBasicMultiChannelPerlin channelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            channelPerlin.m_AmplitudeGain = intensity;
            startingIntensity = intensity;
            shakeTimer = time;
            shakeTimerTotal = time;
        }

        private void Update()
        {
            //TakeScreenShoot();

            if (shakeTimer>0)
            {
                shakeTimer -= Time.deltaTime;
                if(shakeTimer<=0)
                {
                    //timeOver
                    CinemachineBasicMultiChannelPerlin channelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    channelPerlin.m_AmplitudeGain = 0;
                    //Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                }
            }        

            if (climbCam != null)
            {
                if (playerManager.isClimbing && !climbCam.activeInHierarchy)
                {
                    mainCam.SetActive(false);
                    climbCam.SetActive(true);
                }
            }

            if (mainCam != null)
            {
                if (playerManager.isGrounded&&!playerManager.isClimbing && !mainCam.activeInHierarchy)
                {
                    mainCam.SetActive(true);
                    climbCam.SetActive(false);
                }
            }
         
        }

        public void TakeScreenShoot()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("test");               
                string date = System.DateTime.Now.ToString();
                date = date.Replace("/", "-");
                date = date.Replace(" ", "_");
                date = date.Replace(":", "-");
              // ScreenCapture.CaptureScreenshot(Application.dataPath + "/ScreenShots/SS_" + date + ".png");
            }
        }


        #region oldCam
        /*
        [Header("Camera")]
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private CinemachineVirtualCamera virtualCamera=null;

        private CinemachineTransposer transposer;

        private void Start()
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            virtualCamera.gameObject.SetActive(true);

            enabled = true;
        }*/
        #endregion

    }
}