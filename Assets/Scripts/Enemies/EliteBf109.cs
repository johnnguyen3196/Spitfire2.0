using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBf109 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;
    private float burstUpdate = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private game game;
    private GameObject player;

    private float speed = 2.5f;
    private int attackSpeed = 2;
    private int burstAmount = 0;
    private float burstInterval = 0.5f;

    public int currentHealth;
    private Vector3 targetVector;
    private int points;

    private Animation anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        game = GameObject.Find("Game").GetComponent<game>();

        anim = gameObject.GetComponent<Animation>();

        player = GameObject.Find("plane");

        points = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(nextUpdate <= Time.time)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
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

    void Attack()
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

    void Move()
    {
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
        game.notifyKill(points, "BF109F");

        FindObjectOfType<AudioManager>().Play("Explosion");
        FindObjectOfType<DialogueManager>().CreateEnemyDeathText(go1);
    }
}
