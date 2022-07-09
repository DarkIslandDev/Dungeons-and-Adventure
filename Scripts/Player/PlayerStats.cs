using UnityEngine;

public class PlayerStats : MonoBehaviour
 {
     public int healthLevel = 10;
     public int maxHealth;
     public int currentHealth;

     public int staminaLevel = 10;
     public int maxStamina;
     public int currentStamina;

     public HealthBar healthBar;
     public StaminaBar staminaBar;

     private AnimatorHandler animatorHandler;

     private void Start()
     {         
         animatorHandler = GetComponentInChildren<AnimatorHandler>();

         healthBar = UIManager.instance.HUD.healthBar;
         staminaBar = UIManager.instance.HUD.staminaBar;
         
         maxHealth = SetMaxHealthFromHealthLevel();
         currentHealth = maxHealth;
         healthBar.SetMaxHealth(maxHealth);

         maxStamina = SetMaxStaminaFromstaminaLevel();
         currentStamina = maxStamina;
         staminaBar.SetMaxStamina(maxStamina);
     }

     private int SetMaxHealthFromHealthLevel()
     {
         maxHealth = healthLevel * 10;
         return maxHealth;
     }
     
     private int SetMaxStaminaFromstaminaLevel()
     {
         maxStamina = staminaLevel * 10;
         return maxStamina;
     }

     public void TakeDamage(int damage)
     {
         currentHealth = currentHealth - damage;
         
         healthBar.SetCurrentHealth(currentHealth);
         
         animatorHandler.PlayTargetAnimation("Damage_01", true);

         if (currentHealth <= 0)
         {
             currentHealth = 0;
             healthBar.SetCurrentHealth(currentHealth);
             animatorHandler.PlayTargetAnimation("Dead_01" , true);
         }
     }

     public void TakeStaminaDamage(int damage)
     {
         currentStamina = currentStamina - damage;
         staminaBar.SetCurrentStamina(currentStamina);
         
     }
 }