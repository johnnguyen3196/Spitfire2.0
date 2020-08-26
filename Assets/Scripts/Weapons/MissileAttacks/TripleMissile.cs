using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TripleMissile : PlayerAttack
{
    public GameObject missilePrefab;
    public TripleMissile()
    {
        this.UISpriteName = "UITripleMissile";
        this.type = Type.Missile;
        this.id = 3;
    }

    public override void Attack(Transform transform)
    {
        if (missilePrefab == null)
        {
            missilePrefab = Resources.Load("PlayerMissileObject") as GameObject;
        }

        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Vector3 leftMissilePos = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
        Vector3 rightMissilePos = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
        GameObject go = GameObject.Instantiate(missilePrefab, middleMissilePos, Quaternion.identity);
        GameObject go1 = GameObject.Instantiate(missilePrefab, leftMissilePos, Quaternion.identity);
        GameObject go2 = GameObject.Instantiate(missilePrefab, rightMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile1 = go1.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile2 = go2.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
        playerMissile1.damage = 10;
        playerMissile2.damage = 10;
    }
}
