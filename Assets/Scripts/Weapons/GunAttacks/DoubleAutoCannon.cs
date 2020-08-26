using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleAutoCannon : PlayerAttack
{
    public GameObject bulletPrefab;
    public DoubleAutoCannon()
    {
        this.UISpriteName = "UIDoubleAutoCannon";
        this.type = Type.Gun;
        this.id = 8;
    }

    public override void Attack(Transform transform)
    {
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load("PlayerBulletObject") as GameObject;
        }

        Vector3[] positions = { new Vector3(transform.position.x - .4f, transform.position.y + 0.5f, 0), new Vector3(transform.position.x - .3f, transform.position.y + 0.5f, 0), new Vector3(transform.position.x -.2f, transform.position.y + 0.5f, 0) };
        GameObject go = GameObject.Instantiate(bulletPrefab, positions[Random.Range(0, positions.Length)], Quaternion.identity);
        PlayerBullet bullet1 = go.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet1.damage = 10;

        Vector3[] positions1 = { new Vector3(transform.position.x + .4f, transform.position.y + 0.5f, 0), new Vector3(transform.position.x + .3f, transform.position.y + 0.5f, 0), new Vector3(transform.position.x + .2f, transform.position.y + 0.5f, 0) };
        GameObject go1 = GameObject.Instantiate(bulletPrefab, positions1[Random.Range(0, positions.Length)], Quaternion.identity);
        PlayerBullet bullet2 = go1.GetComponent<PlayerBullet>();
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet2.speed = 200;
        bullet2.damage = 10;
    }
}
