using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMissile : PlayerAttack
{
    GameObject missilePrefab;
    public SingleMissile(GameObject missilePrefab)
    {
        this.missilePrefab = missilePrefab;
    }

    public void Attack(Transform transform)
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(missilePrefab, middleMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
    }
}
