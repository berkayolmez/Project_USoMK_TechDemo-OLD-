using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class WeaponHandlerManager : MonoBehaviour
    {
        WeaponHandler leftHandSlot;
        WeaponHandler rightHandSlot;

        public DamageCollider leftDamageCollider;
        public DamageCollider rightDamageCollider;

        private void Awake()
        {
            WeaponHandler[] weaponSlots = GetComponentsInChildren<WeaponHandler>();

            foreach(WeaponHandler weaponSlot in weaponSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                leftDamageCollider = LoadDamageCollider(leftHandSlot);
              
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                rightDamageCollider = LoadDamageCollider(rightHandSlot);
            }
        }

        private DamageCollider LoadDamageCollider(WeaponHandler currentHandSlot)
        {
            DamageCollider findCollider=currentHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

            Debug.Log(findCollider.currentWeaponDamage);
            return findCollider;
        }

        

        public void SetOnLeftDamageCollider()
        {
            leftDamageCollider.EnableDamageCollider();
        }

        public void SetOnRightDamageCollider()
        {
            rightDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftDamageCollider.DisableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightDamageCollider.DisableDamageCollider();
        }


        /*
        private void LoadLeftHandDamageCollider()
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightHandDamageCollider()
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }*/




    }
}