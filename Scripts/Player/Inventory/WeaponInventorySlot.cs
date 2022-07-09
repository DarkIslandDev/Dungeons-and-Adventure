using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
    public Image Icon;
    private WeaponItem item;

    public void AddItem(WeaponItem newItem)
    {
        item = newItem;
        Icon.sprite = item.itemIcon;
        Icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        Icon = null;
        Icon.enabled = false;
        gameObject.SetActive(false);
    }
}