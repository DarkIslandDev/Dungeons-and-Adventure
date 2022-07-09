using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Анимации простоя")] 
    public string right_hand_idle;
    public string left_hand_idle;
    
    [Header("Анимации атаки")]
    public string OH_LightAttack_01;
    public string OH_LightAttack_02;
    public string OH_LightAttack_03;
    public string OH_LightAttack_04;
    
    public string OH_HeavyAttack_01;

    [Header("Stamina Cost")] 
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

}