using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootTypeUI : MonoBehaviour
{
    public Image image;

    public void ChangeShootSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
