using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMissile : PlayerAttack
{
    GameObject missilePrefab;
    public DoubleMissile(GameObject missilePrefab)
    {
        this.missilePrefab = missilePrefab;
    }

    public void Attack(Transform transform)
    {
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
