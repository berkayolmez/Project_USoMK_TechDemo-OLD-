using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace project_WAST
{
    public class HoldOnButtonUI : MonoBehaviour
    {
        [Header("UI Objects")]  //UI for demo 
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Image circle;

        HoldOnButton holdButton;

        private void Awake() //Starting settings
        {
            holdButton = GetComponent<HoldOnButton>();
            if (loadingText != null)
            {
                loadingText.text = "LOADING: %0";
            }

            if(timerSlider!=null)
            {
                timerSlider.value = holdButton.timerSpeed;
            }
        }

        private void Update()
        {
            circle.fillAmount = holdButton.loadingValue;
            if(loadingText!=null)
            {
                loadingText.text = "LOADING: %" + (holdButton.loadingValue * 100).ToString("F0");
            }
        }

        public void SetTimerSpeed()
        {
            if(timerSlider!=null)
            {
                holdButton.timerSpeed = timerSlider.value;
                timerText.text = "TIMER: " + holdButton.timerSpeed.ToString("F0");
            }
        }

        
    }
}