using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyPlane : MonoBehaviour
{
    private int nextUpdate = 0;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float speed;
    public int attackSpeed;
    public int currentHealth;
    public int type;
    public Vector3 targetVector;
    private string[] targetNames = new string[] {"plane"};
    public GameObject explosionPrefab;
    public GameObject player;
    private Rigidbody2D rb;
    private bool rotate;
    //false left, true right
    private bool rotateDirection;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("plane");

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        switch (type)
        {
            case 1:
                speed = 2.5f;
                attackSpeed = 2;
                currentHealth = 10;
                break;
            case 2:
                speed = 2.5f;
                attackSpeed = 2;
                currentHealth = 20;
                break;
            case 3:
                speed = 3f;
                attackSpeed = 2;
                currentHealth = 50;
                break;
            case 4:
                speed = 3.5f;
                attackSpeed = 5;
                currentHealth = 30;
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                rotate = true;
                if(transform.position.x > 0)
                {
                    rotateDirection = false;
                } else
                {
                    rotateDirection = true;
                }
                break;
            case 5:
                speed = 4f;
                attackSpeed = 2;
                currentHealth = 20;
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
                break;
        }

        // find our RigidBody
        rb = gameObject.GetComponent<Rigidbody2D>();
        // add force 
        //targetVector = new Vector3(0, -1, 0);
        //rb.AddForce(targetVector.normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
        Move();
    }

    private void Attack()
    {
        switch (type)
        {
            case 1:
                Enemy1AttackPattern();
                break;
            case 2:
                Enemy2AttackPattern();
                break;
            case 3:
                Enemy3AttackPattern();
                break;
            case 4:
                Enemy4AttackPattern();
                break;
            case 5:
                Enemy5AttackPattern();
                break;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject go1 = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
    }

    private void Move()
    {
        switch (type)
        {
            case 1:
                Enemy1MovementPattern();
                break;

            case 2:
                Enemy2MovementPattern();
                break;
            case 3:
                Enemy3MovementPattern();
                break;
            case 4:
                Enemy4MovementPattern();
                break;
            case 5:
                Enemy5MovementPattern();
                break;
        }
    }

    //Simply moves in a downwards direction
    private void Enemy1MovementPattern()
    {
        rb.velocity = new Vector3(0, -1, 0) * speed;
    }

    //Simply moves down and follows player horizontally
    private void Enemy2MovementPattern()
    {
        Vector3 velocity;
        if(player.transform.position.x < gameObject.transform.position.x)
        {
            velocity = new Vector3(-1, -1, 0) * speed;
        } else
        {
            velocity = new Vector3(1, -1, 0) * speed;
        }
        rb.velocity = velocity;
    }

    //Simply moves in a downward direction
    private void Enemy3MovementPattern()
    {
        rb.velocity = new Vector3(0, -1, 0) * speed;
    }

    private void Enemy4MovementPattern()
    {
        if (rotate)
        {
            //left
            if (!rotateDirection)
            {
                //stop left rotation past 210 degrees
                if(transform.eulerAngles.z < 195)
                {
                    rotate = false;
                } else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z - .3f, Vector3.forward);
                }
            } else
            {
                //right
                //stop right rotation past 330 degrees
                if(transform.eulerAngles.z > 345)
                {
                    rotate = false;
                } else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + .3f, Vector3.forward);
                }
            }
        }
        if(transform.position.x < bottomLeft.x + 2)
        {
            rotate = true;
            rotateDirection = true;
        }
        if(transform.position.x > topRight.x - 2)
        {
            rotate = true;
            rotateDirection = false;
        }
        rb.velocity = transform.right.normalized * speed;
    }

    private void Enemy5MovementPattern()
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

    private void Enemy1AttackPattern()
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

    private void Enemy2AttackPattern()
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

    private void Enemy3AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.15f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.15f, transform.position.y - .65f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y - .75f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);
        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet3.targetVector = new Vector3(0, -1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet3.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
    }

    private void Enemy4AttackPattern()
    {
        Vector3 missilePos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        GameObject go1 = Instantiate(missilePrefab, missilePos, Quaternion.identity);
    }

    private void Enemy5AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
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
}
