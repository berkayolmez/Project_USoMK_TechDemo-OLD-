using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace project_WAST
{
    public class PlayerCameraController : MonoBehaviour
    {
        public static PlayerCameraController Instance { get; private set; }

        private CinemachineVirtualCamera virtualCam;
        private float startingIntensity;
        private float shakeTimerTotal;
        private float shakeTimer;

        private void Awake()
        {
            Instance = this;
            virtualCam = GetComponent<CinemachineVirtualCamera>();
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
            if(shakeTimer>0)
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