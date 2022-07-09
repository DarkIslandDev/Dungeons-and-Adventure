using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    private int curH;
    private int maxH;
    
    private void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        maxH = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }
    
    public void SetCurrentHealth(int currentHealth)
    {
        curH = currentHealth;
        healthSlider.value = currentHealth;

        healthText.text = curH + " / " + maxH;
    }
}