using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Instance

    public static UIManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("у тебя тут где-то ещё один такой же скрипт. Найди его быстрее и удали к хуям собачим, а может быть лишний скрипт это Я.");
            return;
        }

        instance = this;
    }

    #endregion

    public HUD HUD;
    public Menu Menu;
    
    
    private void Start()
    {
        HUD = transform.GetChild(0).GetComponent<HUD>();
        Menu = transform.GetChild(1).GetComponent<Menu>();
    }
    
    #region HUD

    

    #endregion

    #region Menu

    public void OpenMenu()
    {
        Menu.MenuWindow.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        Menu.MenuWindow.gameObject.SetActive(false);
    }

    public void OpenInventory()
    {
        Menu.Inventory.gameObject.SetActive(true);
    }

    public void CloseInventory()
    {
        Menu.Inventory.gameObject.SetActive(false);
    }

    #endregion
}