using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace project_WAST
{
    public class UI_Key : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
    
        [SerializeField] private Transform container;
        [SerializeField] private Transform keyTemp;

        [SerializeField] int i = 1;

        private void Awake()
        {
            container = transform.Find("container");
            i = 1;
            keyTemp.gameObject.SetActive(false);
        }

        private void Start()
        {
            interactor.KeyChange += Interactor_KeyChange;
        }

        private void Interactor_KeyChange(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            List<RequirementTypes.RequirementType> reqList = interactor.GetKeyList();

            RequirementTypes.RequirementType reqType = reqList[i];
            Transform keyTransform = Instantiate(keyTemp, container);
            keyTransform.gameObject.SetActive(true);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 * i, 0);
            Image keyImage = keyTransform.Find("keyImage").GetComponent<Image>();
            switch (reqType)
            {
                default:
                case RequirementTypes.RequirementType.nothing:
                    keyImage.color = Color.grey;
                    break;
                case RequirementTypes.RequirementType.RedKey:
                    keyImage.color = Color.red;
                    break;
                case RequirementTypes.RequirementType.GreenKey:
                    keyImage.color = Color.green;
                    break;
                case RequirementTypes.RequirementType.BlueKey:
                    keyImage.color = Color.blue;
                    break;

            }

            i++;
        }
    }
}