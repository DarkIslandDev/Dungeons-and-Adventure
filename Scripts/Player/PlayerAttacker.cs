using System;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private AnimatorHandler animatorHandler;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            animatorHandler.animator.SetBool("CanDoCombo", false);
            if (lastAttack == weapon.OH_LightAttack_01)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_LightAttack_02, true);
            }
        }
    }
    
    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;
        animatorHandler.PlayTargetAnimation(weapon.OH_LightAttack_01, true);
        lastAttack = weapon.OH_LightAttack_01;
    }
    
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;
        animatorHandler.PlayTargetAnimation(weapon.OH_HeavyAttack_01, true);
        lastAttack = weapon.OH_HeavyAttack_01;
    }
}