using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleMissile : PlayerAttack
{
    public GameObject missilePrefab;
    public SingleMissile()
    {
        this.UISpriteName = "UISingleMissile";
        this.type = Type.Missile;
        this.id = 1;
        missilePrefab = Resources.Load("PlayerMissileObject") as GameObject;
    }

    public override void Attack(Transform transform)
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(missilePrefab, middleMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
    }
}
