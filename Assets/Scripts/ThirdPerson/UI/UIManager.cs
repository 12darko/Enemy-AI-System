using System;
using System.Collections;
using System.Collections.Generic;
using Pattern;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public PlayerInventory playerInventory;
    public EquipmentWindowUI equipmentWindowUI;
    [Header("UI Windows")]
    public GameObject hudWindows;
    public GameObject selectWindows;
    public GameObject weaponInventoryWindow;
    public GameObject equipmentWindows;

    [Header("Equipment Window Slot Selection")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    
    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotParent;
    public WeaponInventorySlot[] weaponInventorySlots;

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
    }


    public void UpdateUI()
    {
        #region Weapon Inventory Slots

        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotParent);
                    weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
                }   
                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion
    }
    
    
    public void OpenSelectWindows()
    {
        selectWindows.SetActive(true);
    }
    public void CloseSelectWindows()
    {
        selectWindows.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentWindows.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }
}
