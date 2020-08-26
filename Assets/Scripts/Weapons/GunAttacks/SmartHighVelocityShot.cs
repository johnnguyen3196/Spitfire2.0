using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmartHighVelocityShot : PlayerAttack
{
    public GameObject bulletPrefab;
    public SmartHighVelocityShot()
    {
        this.UISpriteName = "UISmartHighVelocityShot";
        this.type = Type.Gun;
        this.id = 9;
        bulletPrefab = Resources.Load("PlayerBulletObject") as GameObject;
    }

    public override void Attack(Transform transform)
    {
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = GameObject.Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go.GetComponent<PlayerBullet>();
        bullet1.targetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        bullet1.speed = 750;
        bullet1.damage = 100;
    }
}
