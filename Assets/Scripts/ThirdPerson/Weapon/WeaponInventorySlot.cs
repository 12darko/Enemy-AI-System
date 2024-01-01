using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.Weapon;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private UIManager _uiManager;
    private WeaponSlotManager _weaponSlotManager;
    
    public Image icon;
    private WeaponItem _item;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        _uiManager = FindObjectOfType<UIManager>();
        _weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
        _item = newItem;
        icon.sprite = _item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        _item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);

    }

    public void EquipThisItem()
    {
        if (_uiManager.rightHandSlot01Selected)
        {
            _playerInventory.weaponsInventory.Add(_playerInventory.weaponsInRightHandSlots[0]);
            _playerInventory.weaponsInRightHandSlots[0] = _item;
            _playerInventory.weaponsInventory.Remove(_item);
        }else if (_uiManager.rightHandSlot02Selected)
        {
            _playerInventory.weaponsInventory.Add(_playerInventory.weaponsInRightHandSlots[1]);
            _playerInventory.weaponsInRightHandSlots[1] = _item;
            _playerInventory.weaponsInventory.Remove(_item);
        }
        else if (_uiManager.leftHandSlot01Selected)
        {
            _playerInventory.weaponsInventory.Add(_playerInventory.weaponsInLeftHandSlots[0]);
            _playerInventory.weaponsInLeftHandSlots[0] = _item;
            _playerInventory.weaponsInventory.Remove(_item);
        }
        else if(_uiManager.leftHandSlot02Selected)
        {
            _playerInventory.weaponsInventory.Add(_playerInventory.weaponsInLeftHandSlots[1]);
            _playerInventory.weaponsInLeftHandSlots[1] = _item;
            _playerInventory.weaponsInventory.Remove(_item);
        }
        else
        {
            return;
        }
        _playerInventory.rightWeapon =
            _playerInventory.weaponsInRightHandSlots[_playerInventory.currentRightWeaponIndex];
        _playerInventory.leftWeapon =
            _playerInventory.weaponsInLeftHandSlots[_playerInventory.currentLeftWeaponIndex];
        _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.rightWeapon, false);
        _weaponSlotManager.LoadWeaponOnSlot(_playerInventory.leftWeapon, true);
        _uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(_playerInventory);
        _uiManager.ResetAllSelectedSlots();
       
    }
}
