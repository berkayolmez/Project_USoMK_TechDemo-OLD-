using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace project_WAST
{
    public class UI_Reqs : MonoBehaviour
    {
        RequirementObjects requirementObjects;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private float loadingValue = 0;
        [SerializeField] private Image circle;


        private void Awake()
        {
            requirementObjects = GetComponent<RequirementObjects>();
            loadingText.text = "%0";          
        }
               
        void Update()
        {
            if(!requirementObjects.myStatus) //UI bazen 99da kalýyor bunu çöz
            {
                loadingValue = requirementObjects.myLoadValue;
                circle.fillAmount = loadingValue/requirementObjects.maxLoadValue;
                loadingText.text = "%" + (circle.fillAmount*100).ToString("F0");
            }
            else if(requirementObjects.myStatus)
            {
                loadingValue = requirementObjects.maxLoadValue;
                circle.fillAmount = 1;
                loadingText.text = "%" + (100).ToString("F0");
            }   
        }

    }
}