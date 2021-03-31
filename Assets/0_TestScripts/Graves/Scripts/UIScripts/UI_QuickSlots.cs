using UnityEngine;
using UnityEngine.UI;

namespace project_WAST
{
    public class UI_QuickSlots : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponSlotUI(bool isLeft,SpellItem spell)
        {
            if(isLeft)
            {
                if (spell.itemIcon != null)
                {
                    leftWeaponIcon.sprite = spell.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
                  
            }
            else
            {
                if(spell.itemIcon!=null)
                {
                    rightWeaponIcon.sprite = spell.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false; //gerek var mii?????
                }
            }
        }
    }
}