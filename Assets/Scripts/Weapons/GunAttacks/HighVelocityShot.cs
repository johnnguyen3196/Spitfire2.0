using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighVelocityShot : PlayerAttack
{
    GameObject bulletPrefab;
    public HighVelocityShot(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }

    public void Attack(Transform transform)
    {
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 750;
        bullet1.damage = 75;
    }
}
