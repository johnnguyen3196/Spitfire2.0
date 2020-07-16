using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StanceUI : MonoBehaviour
{
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI missileText;
    public TextMeshProUGUI shieldText;

    private void SetDefault(TextMeshProUGUI text)
    {
        text.color = new Color32(255, 255, 0, 150);
        text.fontSize = 30;
    }

    private void SelectText(TextMeshProUGUI text)
    {
        text.color = new Color32(255, 0, 0, 255);
        text.fontSize = 36;
    }

    public void SelectAgility()
    {
        SelectText(agilityText);
        SetDefault(gunText);
        SetDefault(missileText);
        SetDefault(shieldText);
    }

    public void SelectGun()
    {
        SelectText(gunText);
        SetDefault(agilityText);
        SetDefault(missileText);
        SetDefault(shieldText);
    }

    public void SelectMissile()
    {
        SelectText(missileText);
        SetDefault(agilityText);
        SetDefault(gunText);
        SetDefault(shieldText);
    }

    public void SelectShield()
    {
        SelectText(shieldText);
        SetDefault(agilityText);
        SetDefault(gunText);
        SetDefault(missileText);
    }
}
