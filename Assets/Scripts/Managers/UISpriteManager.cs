﻿using System;
using UnityEngine;

public class UISpriteManager : MonoBehaviour
{
    public SpriteObject[] Sprites;

    public UISpriteManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public Sprite Find(string name)
    {
        SpriteObject spriteObject = Array.Find(Sprites, o => o.name == name);
        if(spriteObject == null)
        {
            UnityEngine.Debug.LogWarning("Sprite: " + name + " not found!");
            return null;
        }
        return spriteObject.sprite;
    }
}
