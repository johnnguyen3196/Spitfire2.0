using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Me163 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;
    private float burstUpdate = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private game game;

    private float speed = 4f;
    private int attackSpeed = 2;
    private int burstAmount = 0;
    private float burstInterval = 0.25f;
    public int currentHealth = 30;
    private Vector3 targetVector;
    private int points;

    private Rigidbody2D rb;
    private Animation anim;

    private bool rotate;
    //false left, true right
    private bool rotateDirection;

    private Vector3 bottomLeft;
    private Vector3 topRight;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        anim = gameObject.GetComponent<Animation>();

        game = GameObject.Find("Game").GetComponent<game>();

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

        points = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextUpdate <= Time.time)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
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

    void Attack()
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

    void Move()
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

    public int TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.Play("EnemyDamageAnimation");
        return currentHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject go1 = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        game.notifyKill(points, "Me163");

        FindObjectOfType<AudioManager>().Play("Explosion");
        FindObjectOfType<DialogueManager>().CreateEnemyDeathText(go1);
    }
}
