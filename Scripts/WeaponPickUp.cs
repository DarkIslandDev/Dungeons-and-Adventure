using UnityEngine;

public class WeaponPickUp : Interactable
{
    public WeaponItem weaponItem;

    private void Start()
    {
        interactableText = weaponItem.name;
        // itemIcon.sprite = weaponItem.itemIcon;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;
        
        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();
        
        playerLocomotion.rigidbody.velocity = Vector3.zero;
        
        animatorHandler.PlayTargetAnimation("Pick Up Item Start", true);
        
        playerInventory.weaponsInventory.Add(weaponItem);
        
        // itemIcon.sprite = weaponItem.itemIcon;
        
        playerManager.isInteracting = false;
        Destroy(gameObject);
    }
}