using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Weapon;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    [SerializeField] private Image leftWeaponIcon;
    [SerializeField] private Image rightWeaponIcon;
    
    #region Props

    public Image LeftWeaponIcon
    {
        get => leftWeaponIcon;
        set => leftWeaponIcon = value;
    }

    public Image RightWeaponIcon
    {
        get => rightWeaponIcon;
        set => rightWeaponIcon = value;
    }

    #endregion


    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weaponItem)
    {
        if (isLeft == false)
        {
            if (weaponItem.itemIcon != null)
            {
                rightWeaponIcon.sprite = weaponItem.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
  
        }
        else
        {
            if (weaponItem.itemIcon != null)
            {
                leftWeaponIcon.sprite = weaponItem.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
    }
}
