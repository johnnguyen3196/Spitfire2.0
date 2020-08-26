using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squadron : Escort
{
    public GameObject bulletPrefab;

    public Squadron()
    {
        
    }

    public override void Attack()
    {
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
        GameObject go = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        PlayerBullet bullet = go.GetComponent<PlayerBullet>();
        bullet.targetVector = new Vector3(0, 1, 0);
        bullet.speed = 200;
        bullet.damage = 10;
    }
}
