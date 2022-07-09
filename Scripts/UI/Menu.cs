using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Transform MenuWindow;
    public Transform Inventory;

    public InventoryUI inventoryUI;

    private void Start()
    {
        MenuWindow = transform.GetChild(0);
        MenuWindow.gameObject.SetActive(false);

        Inventory = transform.GetChild(1);
        inventoryUI = Inventory.GetComponent<InventoryUI>();
        Inventory.gameObject.SetActive(false);
    }
}