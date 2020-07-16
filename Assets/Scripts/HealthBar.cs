using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health.ToString();
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        health = Mathf.Floor(health);
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(health < 0)
        {
            health = 0;
        }
        healthText.text = health.ToString();
    }
}
