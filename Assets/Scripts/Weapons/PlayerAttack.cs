using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack
{
    protected string UISpriteName = "Empty";

    public enum Type
    {
        Gun,
        Missile,
        Escort
    }

    public Type type;

    public int id = 0;

    public virtual void Attack(Transform transform) {

    }

    public Sprite GetSprite()
    {
        return GameObject.FindObjectOfType<UISpriteManager>().Find(UISpriteName);
    }
}
