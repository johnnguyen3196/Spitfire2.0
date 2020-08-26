using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleMissile : PlayerAttack
{
    public GameObject missilePrefab;
    public DoubleMissile()
    {
        this.UISpriteName = "UIDoubleMissile";
        this.type = Type.Missile;
        this.id = 2;
    }

    public override void Attack(Transform transform)
    {
        if (missilePrefab == null)
        {
            missilePrefab = Resources.Load("PlayerMissileObject") as GameObject;
        }

        Vector3 leftMissilePos = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
        Vector3 rightMissilePos = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
        GameObject go1 = GameObject.Instantiate(missilePrefab, leftMissilePos, Quaternion.identity);
        GameObject go2 = GameObject.Instantiate(missilePrefab, rightMissilePos, Quaternion.identity);
        PlayerMissile playerMissile1 = go1.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile2 = go2.GetComponent<PlayerMissile>();
        playerMissile1.damage = 10;
        playerMissile2.damage = 10;
    }
}
