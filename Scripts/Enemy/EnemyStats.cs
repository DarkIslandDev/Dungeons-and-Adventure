using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        private Animator animator;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();

            maxHealth = SetMaxHealthFromLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamae(int damage)
        {
            currentHealth = currentHealth - damage;
            
            animator.Play("Damage_01");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");
            }
        }
    }
}