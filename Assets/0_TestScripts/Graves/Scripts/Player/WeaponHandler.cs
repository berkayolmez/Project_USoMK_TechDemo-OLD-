using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class WeaponHandler : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon(bool canDelete)
        {
            if(currentWeaponModel!=null)
            {
                if(canDelete)
                {
                    Destroy(currentWeaponModel);
                }
                else
                {
                    currentWeaponModel.SetActive(false);
                }
            }
        }

        public void LoadWeaponModel(WeaponItem weapon)
        {
            UnloadWeapon(true); //unload and destory

            if (weapon==null)
            {
                UnloadWeapon(false); //just unload
                return;
            }

            if(weapon.modelPrefab!=null)
            {
                GameObject model = Instantiate(weapon.modelPrefab) as GameObject;

                if (model != null)
                {
                    if (parentOverride != null)
                    {
                        model.transform.parent = parentOverride;
                    }
                    else
                    {
                        model.transform.parent = transform;
                    }

                    model.transform.localPosition = Vector3.zero;
                    model.transform.localRotation = Quaternion.identity;
                    model.transform.localScale = Vector3.one;
                }

                currentWeaponModel = model;
            }

           
        }

    }
}