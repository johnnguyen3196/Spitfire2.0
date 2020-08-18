using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBf109 : EnemyPlane
{
    private float burstUpdate = 0;

    public int burstAmount = 0;
    public float burstInterval = 0.5f;

    private GameObject player;

    public GameObject bulletPrefab;

    public override void InitializeEnemy()
    {
        player = GameObject.Find("plane");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(attackUpdate <= Time.time)
        {
            attackUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            burstAmount = 0;
        }
        //Attack pattern
        //Attack ---0.5secs--- Attack ---1.5secs---
        if(burstAmount < 2 && burstUpdate <= Time.time)
        {
            Attack();
            burstUpdate = Time.time + burstInterval;
            burstAmount++;
        }
        Move();
    }

    public override void Attack()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();

        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet3.targetVector = new Vector3(0, -1, 0);

        bullet1.speed = 150;
        bullet2.speed = 150;
        bullet3.speed = 150;

        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
    }

    public override void Move()
    {
        if (player == null)
            return;
        Vector3 velocity;
        if (player.transform.position.x < gameObject.transform.position.x)
        {
            velocity = new Vector3(-.33f, -1, 0) * speed;
        }
        else
        {
            velocity = new Vector3(.33f, -1, 0) * speed;
        }
        rb.velocity = velocity;
    }
}
