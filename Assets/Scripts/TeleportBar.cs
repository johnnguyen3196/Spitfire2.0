using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TeleportBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxTele()
    {
        slider.maxValue = 1;
        slider.value = 1;
    }

    public void SetTele(float percentage)
    {
        slider.value = percentage;
    }
}
