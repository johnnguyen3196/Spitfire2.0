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
        missilePrefab = Resources.Load("PoisonMissileObject") as GameObject;
    }

    public override void Attack(Transform transform)
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(missilePrefab, middleMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
    }
}
