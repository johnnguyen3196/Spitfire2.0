using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsUI : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    public void setPointsText(int points, int total)
    {
        pointsText.text = points.ToString() + "/" + total.ToString(); 
    }
}
