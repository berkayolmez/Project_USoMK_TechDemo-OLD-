using UnityEngine;
using UnityEngine.UI;

namespace project_usomk
{
    public class UI_QuickSlots : MonoBehaviour   //Screen spell/weapon UI manager
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponSlotUI(bool isLeft,SpellItem spell)    
        {
            if(isLeft)
            {
                if (spell.itemIcon != null)
                {
                    leftWeaponIcon.sprite = spell.itemIcon;         //Get icon from spell/weapon 
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
                    rightWeaponIcon.sprite = spell.itemIcon;        //Get icon from spell/weapon 
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
        }
    }
}