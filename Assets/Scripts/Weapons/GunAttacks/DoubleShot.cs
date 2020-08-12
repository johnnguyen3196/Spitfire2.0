using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShot : PlayerAttack
{
    GameObject bulletPrefab;
    public DoubleShot(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;    
    }

    public void Attack(Transform transform)
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        GameObject go1 = GameObject.Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = GameObject.Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
    }
}
