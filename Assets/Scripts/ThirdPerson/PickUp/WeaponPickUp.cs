using ThirdPerson.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThirdPerson.PickUp
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weaponItem;

        public override void Interact(PlayerManager playerManager)
        {
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            PlayerAnimationManager playerAnimationManager;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            playerAnimationManager = playerManager.GetComponentInChildren<PlayerAnimationManager>();
            
            playerLocomotion.Rigidbody.velocity = Vector3.zero; //Oyuncunun hareket etmesini durduruyoruz çünkü yerden item alıyor
            playerAnimationManager.PlayTargetAnimation("Pick Up Item", true); //İtem alma animasyonu
            playerInventory.weaponsInventory.Add(weaponItem);
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<TMP_Text>().text = weaponItem.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weaponItem.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}