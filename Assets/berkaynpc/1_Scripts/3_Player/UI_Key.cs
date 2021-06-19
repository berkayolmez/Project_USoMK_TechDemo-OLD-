using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace project_usomk
{
    public class UI_Key : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private PlayerInventory inventory;
    
        [SerializeField] private Transform container;
        [SerializeField] private Transform keyTemp;

        private void Awake()
        {
            container = transform.Find("container");        //Key holder
            keyTemp.gameObject.SetActive(false);        //Key template
        }

        private void Start()
        {
            interactor.KeyChange += Interactor_KeyChange;
        }

        private void Interactor_KeyChange(object sender, System.EventArgs e)        
        {
            UpdateVisual();
        }

        private void UpdateVisual()     //Update key visual
        {
            foreach(Transform child in container)       //Protection for duplicated keys
            {
                if (child == keyTemp) continue;
                Destroy(child.gameObject);
            }

            List<RequirementTypes.RequirementType> reqList = inventory.KeyList;

            for (int i = 1; i < reqList.Count; i++)
            {
                RequirementTypes.RequirementType reqType = reqList[i];
                Transform keyTransform = Instantiate(keyTemp, container);       //Instantiate new key in container
                keyTransform.gameObject.SetActive(true);                        //Set active true
                keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20 * i, 0);       //Key icon position
                Image keyImage = keyTransform.Find("keyImage").GetComponent<Image>();       //Find target image to change sprite
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