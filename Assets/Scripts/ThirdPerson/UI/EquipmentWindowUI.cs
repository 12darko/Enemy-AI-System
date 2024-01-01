using System;
using System.Collections;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;


    public HandEquipmentSlotUI[] _handEquipmentSlotUI;


    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
        for (int i = 0; i < _handEquipmentSlotUI.Length; i++)
        {
            if (_handEquipmentSlotUI[i].rightHandSlot01)
            {
                _handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
            }
            else if (_handEquipmentSlotUI[i].rightHandSlot02)
            {
                _handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
            }
            else if (_handEquipmentSlotUI[i].leftHandSlot01)
            {
                _handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
            }
            else
            {
                _handEquipmentSlotUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
            }
        }
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot01Selected = true;
    }
}