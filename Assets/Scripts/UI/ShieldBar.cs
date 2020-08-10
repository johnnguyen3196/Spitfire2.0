using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxShield(float shield)
    {
        slider.maxValue = shield;
        slider.value = shield;
    }

    public void SetShield(float shield)
    {
        slider.value = shield;
    }

    public void SetMaxValue(float shield)
    {
        slider.maxValue = shield;
    }
}
