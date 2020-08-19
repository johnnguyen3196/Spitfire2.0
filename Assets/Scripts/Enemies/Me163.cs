using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Me163 : EnemyPlane
{
    private float burstUpdate = 0;

    public GameObject bulletPrefab;

    private int burstAmount = 0;
    private float burstInterval = 0.25f;

    private bool rotate;
    ////false left, true right
    private bool rotateDirection;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    public override void InitializeEnemy()
    {
        transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        rotate = true;
        if (transform.position.x > 0)
        {
            rotateDirection = false;
        }
        else
        {
            rotateDirection = true;
        }

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (attackUpdate <= Time.time)
        {
            attackUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            burstAmount = 0;
        }
        //Attack pattern
        //Attack ---0.25secs--- Attack ---0.25secs---Attack ---0.25secs--- Attack ---1.25secs---
        if (burstAmount < 4 && burstUpdate <= Time.time)
        {
            Attack();
            burstUpdate = Time.time + burstInterval;
            burstAmount++;
        }
        Move();
    }

    public override void Attack()
    {
        //Get the position(location of the two empty gameObjects) of where the bullet will spawn.
        Vector3 leftBulletPos = gameObject.transform.GetChild(1).gameObject.transform.position;
        Vector3 rightBulletPos = gameObject.transform.GetChild(2).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();

        bullet1.targetVector = transform.right;
        bullet2.targetVector = transform.right;

        bullet1.speed = 250;
        bullet2.speed = 250;
        bullet1.damage = 10;
        bullet2.damage = 10;
    }

    public override void Move()
    {
        if (rotate)
        {
            //left
            if (!rotateDirection)
            {
                //stop left rotation past 195 degrees
                if (transform.eulerAngles.z < 195)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z - .5f, Vector3.forward);
                }
            }
            else
            {
                //right
                //stop right rotation past 345 degrees
                if (transform.eulerAngles.z > 345)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + .5f, Vector3.forward);
                }
            }
        }
        if (transform.position.x < bottomLeft.x + 3)
        {
            rotate = true;
            rotateDirection = true;
        }
        if (transform.position.x > topRight.x - 3)
        {
            rotate = true;
            rotateDirection = false;
        }
        rb.velocity = transform.right.normalized * speed;
    }
}
