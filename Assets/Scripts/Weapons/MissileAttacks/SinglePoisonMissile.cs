using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SinglePoisonMissile : PlayerAttack
{
    public GameObject missilePrefab;
    public SinglePoisonMissile()
    {
        this.UISpriteName = "UISlowMissile";
        this.type = Type.Missile;
        this.id = 5;
    }

    public override void Attack(Transform transform)
    {
        if (missilePrefab == null)
        {
            missilePrefab = Resources.Load("PoisonMissileObject") as GameObject;
        }

        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(missilePrefab, middleMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
    }
}
