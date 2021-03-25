using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_WAST
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponHandlerManager weaponHandlerManager;

        public SpellProjectile currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        private void Awake()
        {
            weaponHandlerManager = GetComponentInChildren<WeaponHandlerManager>();
        }

        private void Start()
        {
            weaponHandlerManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponHandlerManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        private void SwitchSpellType()
        {
            //e ve q ile spell degistir.
        }
    }
}