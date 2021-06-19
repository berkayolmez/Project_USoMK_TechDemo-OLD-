using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

namespace project_usomk
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

        /// <summary>
        /// When this method is called, the camera shakes at the given values.
        /// </summary>
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
                if(shakeTimer<=0) //timeOver
                {                   
                    CinemachineBasicMultiChannelPerlin channelPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    channelPerlin.m_AmplitudeGain = 0;
                    //Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                }
            }

            #region Switch between cameras

            if (climbCam != null)
            {
                if (playerManager.isClimbing && !climbCam.activeInHierarchy)        //When player start to climb this camera will be activated.
                {
                    mainCam.SetActive(false);
                    climbCam.SetActive(true);
                }
            }

            if (mainCam != null)
            {
                if (playerManager.isGrounded&&!playerManager.isClimbing && !mainCam.activeInHierarchy)      //Standart main camera
                {
                    mainCam.SetActive(true);
                    climbCam.SetActive(false);
                }
            }

            #endregion

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
                ScreenCapture.CaptureScreenshot(Application.dataPath + "/ScreenShots/SS_" + date + ".png");
            }
        }
    }
}