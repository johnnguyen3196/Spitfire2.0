﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoCannon : PlayerAttack
{
    public GameObject bulletPrefab;
    public AutoCannon()
    {
        this.UISpriteName = "UIAutoCannon";
        this.type = Type.Gun;
        this.id = 5;
        bulletPrefab = Resources.Load("PlayerBulletObject") as GameObject;
    }

    public override void Attack(Transform transform)
    {
        Vector3[] positions = { new Vector3(transform.position.x - .1f, transform.position.y + 0.5f, 0), new Vector3(transform.position.x, transform.position.y + 0.5f, 0), new Vector3(transform.position.x + .1f, transform.position.y + 0.5f, 0) };
        GameObject go = GameObject.Instantiate(bulletPrefab, positions[Random.Range(0, positions.Length)], Quaternion.identity);
        PlayerBullet bullet1 = go.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet1.damage = 10;
    }
}
