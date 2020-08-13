using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Me262 : EnemyPlane
{
    public GameObject missilePrefab;
   
    private GameObject player;

    private bool rotate;
    ////false left, true right
    private bool rotateDirection;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    // Update is called once per frame
    public override void InitializeEnemy()
    {
        player = GameObject.Find("plane");

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

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
    }

    protected override void Update()
    {
        if (Time.time >= attackUpdate)
        {
            attackUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
        Move();
    }

    public override void Attack()
    {
        Vector3 missilePos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        GameObject go1 = Instantiate(missilePrefab, missilePos, Quaternion.identity);
        Missile missile = go1.GetComponent<Missile>();
        missile.targetVector = new Vector3(0, -1, 0);
    }

    public override void Move()
    {
        if (rotate)
        {
            //left
            if (!rotateDirection)
            {
                //stop left rotation past 210 degrees
                if (transform.eulerAngles.z < 195)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z - .3f, Vector3.forward);
                }
            }
            else
            {
                //right
                //stop right rotation past 330 degrees
                if (transform.eulerAngles.z > 345)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + .3f, Vector3.forward);
                }
            }
        }
        if (transform.position.x < bottomLeft.x + 2)
        {
            rotate = true;
            rotateDirection = true;
        }
        if (transform.position.x > topRight.x - 2)
        {
            rotate = true;
            rotateDirection = false;
        }
        rb.velocity = transform.right.normalized * speed;
    }
}
