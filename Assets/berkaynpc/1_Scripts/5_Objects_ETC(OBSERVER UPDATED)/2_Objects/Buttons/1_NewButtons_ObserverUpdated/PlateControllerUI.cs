using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace project_usomk
{
    public class PlateControllerUI : MonoBehaviour
    {
        [Header("UI Objects")]      //UI for demo 
        [SerializeField] private TextMeshProUGUI loadingText;       //for loadings current value as text
        [SerializeField] private TextMeshProUGUI timerText;         //Loading speed
        [SerializeField] private Slider timerSlider;        //Set loading speed with slider
        [SerializeField] private Image circle;      //The circle around the pressure plate as loader visualization.

        PlateController plateController;

        private void Awake()
        {
            plateController = GetComponent<PlateController>();
            if (loadingText != null)
            {
                loadingText.text = "LOADING: %0";
            }

            if(timerSlider!=null)
            {
                timerSlider.value = plateController.timerSpeed;
            }
        }

        private void Update()
        {
            circle.fillAmount = plateController.loadingValue;
            if(loadingText!=null)
            {
                loadingText.text = "LOADING: %" + (plateController.loadingValue * 100).ToString("F0");
            }
        }

        //Set loading speed with slider
        public void SetTimerSpeed()
        {
            if(timerSlider!=null)
            {
                plateController.timerSpeed = timerSlider.value;
                timerText.text = "TIMER: " + plateController.timerSpeed.ToString("F0");
            }
        }

        
    }
}