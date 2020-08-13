using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBF109 : EnemyPlane
{
    public GameObject bulletPrefab;

    public override void InitializeEnemy()
    {
        this.rb.velocity = new Vector3(0, -1, 0) * speed;
    }

    public override void Attack()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();

        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);

        bullet1.speed = 150;
        bullet2.speed = 150;

        bullet1.damage = 10;
        bullet2.damage = 10;
    }
}
