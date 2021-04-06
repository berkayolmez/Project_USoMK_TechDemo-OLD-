using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace project_WAST
{
    public class UI_Key : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private PlayerInventory inventory;
    
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
            foreach(Transform child in container)
            {
                if (child == keyTemp) continue;
                Destroy(child.gameObject);
            }

            List<RequirementTypes.RequirementType> reqList = inventory.KeyList;

            for (int i = 1; i < reqList.Count; i++)
            {
                RequirementTypes.RequirementType reqType = reqList[i];
                Transform keyTransform = Instantiate(keyTemp, container);
                keyTransform.gameObject.SetActive(true);
                keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20 * i, 0);
                Image keyImage = keyTransform.Find("keyImage").GetComponent<Image>();
                switch (reqType)
                {
                    default:
                    case RequirementTypes.RequirementType.nothing:
                        keyImage.color = Color.grey;
                        break;
                    case RequirementTypes.RequirementType.RedKey:
                        keyImage.color = new Color(0.74f, 0.078f, 0.2f);
                        break;
                    case RequirementTypes.RequirementType.GreenKey:
                        keyImage.color = new Color(0.22f, 0.67f, 0.27f);
                        break;
                    case RequirementTypes.RequirementType.BlueKey:
                        keyImage.color = new Color(0f, 0.31f, 0.93f);
                        break;

                }
            }
        }
    }
}