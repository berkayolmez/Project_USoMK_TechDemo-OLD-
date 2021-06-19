using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace project_usomk
{
    public class UI_Reqs : MonoBehaviour
    {
        RepairableObjects repairableObjects;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private float loadingValue = 0;
        [SerializeField] private Image circle;

        private void Awake()
        {
            repairableObjects = GetComponent<RepairableObjects>();
            loadingText.text = "%0";          
        }
               
        void Update()
        {
            if(!repairableObjects.myStatus) //UI bazen 99da kalýyor bunu çöz
            {
                loadingValue = repairableObjects.myLoadValue;
                circle.fillAmount = loadingValue/ repairableObjects.maxLoadValue;
                loadingText.text = "%" + (circle.fillAmount*100).ToString("F0");
            }
            else if(repairableObjects.myStatus)
            {
                loadingValue = repairableObjects.maxLoadValue;
                circle.fillAmount = 1;
                loadingText.text = "%" + (100).ToString("F0");
            }   
        }

    }
}