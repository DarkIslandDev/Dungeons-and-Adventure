using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    private void Start()
    {
        staminaSlider = GetComponent<Slider>();
    }

    public void SetMaxStamina(int maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }

    public void SetCurrentStamina(int currentStamina)
    {
        staminaSlider.value = currentStamina;
    }
}