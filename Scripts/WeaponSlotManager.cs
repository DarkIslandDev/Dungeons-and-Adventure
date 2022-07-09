using System;
using UnityEngine;


public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;

    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    public WeaponItem attackingWeapon;
    
    private Animator animator;

    private QuickSlotsUI quickSlotsUI;

    private PlayerStats playerStats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponentInParent<PlayerStats>();
        
        quickSlotsUI = UIManager.instance.HUD.quickSlotsUI;
        
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            
            // UIManager.instance.HUD.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            #region Handle Left Weapon Idle Animation
            
            if(weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
            }
            
            #endregion
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            // UIManager.instance.HUD.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);

            #region Handle Right Weapon Idle Animation
            
            if(weaponItem != null)
            {
                animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
            }
            
            #endregion
        }
    }

    #region Handle Weapon`s Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    
    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    
    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }
    
    #endregion

    #region Handle Weapon`s Stamina Drainage
    
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    
    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    
    #endregion
    
    
}