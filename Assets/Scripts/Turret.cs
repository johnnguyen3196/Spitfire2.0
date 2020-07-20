﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float attackWindow = 0;
    private float attackWindowUpdate = 0;
    private float attackSpeed;
    private float attackUpdate = 0;

    public GameObject player;

    void Start()
    {
        player = GameObject.Find("plane");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > attackWindowUpdate)
        {
            SetAttackPattern();
            attackWindowUpdate = Mathf.FloorToInt(Time.time) + attackWindow;
        }

        if(Time.time > attackUpdate)
        {
            Attack();
            attackUpdate = Time.time + attackSpeed;
        }
        //rotate turret to look at player
        transform.right = player.transform.position - transform.position;

    }

    void Attack()
    {
        Vector3 leftBulletPos = gameObject.transform.GetChild(1).gameObject.transform.position;
        Vector3 rightBulletPos = gameObject.transform.GetChild(2).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();

        bullet1.targetVector = (player.transform.position - transform.position).normalized;
        bullet2.targetVector = (player.transform.position - transform.position).normalized;

        bullet1.speed = 150;
        bullet2.speed = 150;
        bullet1.damage = 10;
        bullet2.damage = 10;
    }

    void SetAttackPattern()
    {
        switch(Random.Range(0, 2))
        {
            case 0:
                attackSpeed = 2;
                attackWindow = 10;
                break;
            case 1:
                attackSpeed = .5f;
                attackWindow = 2;
                break;
        }
    }
}
