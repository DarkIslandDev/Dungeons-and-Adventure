using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public Transform Inventory;
    
    [Header("Weapon Inventory")]
    public GameObject weaponInventoryPrefab;
    public Transform weaponInventorySlotParent;
    
    private WeaponInventorySlot[] weaponInventorySlots;

    private void Start()
    {
        playerInventory = PlayerManager.instance.inputHandler.inventory;
        
        Inventory = transform.GetChild(0);
        weaponInventorySlotParent = Inventory.transform.GetChild(1);

        weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
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
                    Instantiate(weaponInventoryPrefab, weaponInventorySlotParent);
                    // weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
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
}