using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuadShot : PlayerAttack
{
    public GameObject bulletPrefab;
    public QuadShot()
    {
        this.UISpriteName = "UIQuadShot";
        this.type = Type.Gun;
        this.id = 3;
    }

    public override void Attack(Transform transform)
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load("PlayerBulletObject") as GameObject;
        }

        Vector3 leftLeftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightRightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 middleLeftBulletPos = new Vector3(transform.position.x - .1f, transform.position.y + .4f, transform.position.z);
        Vector3 middleRightBulletPos = new Vector3(transform.position.x + .1f, transform.position.y + .4f, transform.position.z);
        GameObject go1 = GameObject.Instantiate(bulletPrefab, leftLeftBulletPos, Quaternion.identity);
        GameObject go2 = GameObject.Instantiate(bulletPrefab, rightRightBulletPos, Quaternion.identity);
        GameObject go3 = GameObject.Instantiate(bulletPrefab, middleLeftBulletPos, Quaternion.identity);
        GameObject go4 = GameObject.Instantiate(bulletPrefab, middleRightBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        PlayerBullet bullet3 = go3.GetComponent<PlayerBullet>();
        PlayerBullet bullet4 = go4.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet3.targetVector = new Vector3(0, 1, 0);
        bullet4.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet3.speed = 200;
        bullet4.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
        bullet4.damage = 10;
    }
}
