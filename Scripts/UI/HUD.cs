using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public QuickSlotsUI quickSlotsUI;
    public InteractableUI interactableUI;

    private void Start()
    {
        healthBar = transform.GetChild(0).GetComponent<HealthBar>();
        staminaBar = transform.GetChild(1).GetComponent<StaminaBar>();
        quickSlotsUI = transform.GetChild(2).GetComponent<QuickSlotsUI>();
        interactableUI = transform.GetChild(3).GetComponent<InteractableUI>();
    }
}