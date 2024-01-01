using System;
using System.Collections;
using System.Collections.Generic;
using ThirdPerson.PickUp;
using ThirdPerson.Weapon;
using UnityEngine;

public class OpenChest : Interactable
{
    private Animator _animator;
    private OpenChest _openChest;
    
    public Transform playerStandingPosition;
    public GameObject itemSpawner;
    public WeaponItem itemInChest;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _openChest = GetComponent<OpenChest>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();
        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300* Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        playerManager.OpenChestInteraction(playerStandingPosition);
        _animator.Play("Chest Open");
        StartCoroutine(SpawnItemInChest());
        WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();
        if (weaponPickUp != null)
        {
            weaponPickUp.weaponItem = itemInChest;
        }

    }

    private IEnumerator SpawnItemInChest()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(itemSpawner, transform);
        Destroy(_openChest);
    }
}
