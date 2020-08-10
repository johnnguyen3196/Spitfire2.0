using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class He111 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;
    private float burstUpdate = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private game game;
    private GameObject player;

    private float speed = 1;
    private int attackSpeed = 2;
    private int burstAmount = 0;
    private float burstInterval = 0.25f;

    public int currentHealth;
    private Vector3 targetVector;
    private int points;

    private Rigidbody2D rb;
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        anim = gameObject.GetComponent<Animation>();

        rb.velocity = new Vector3(0, -1, 0) * speed;

        game = GameObject.Find("Game").GetComponent<game>();

        player = GameObject.Find("plane");

        points = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextUpdate <= Time.time)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            BodyAttack();
            burstAmount = 0;
        }
        //Attack pattern
        //Attack ---0.25secs--- Attack ---0.25secs---Attack ---0.25secs--- Attack ---1.25secs---
        if (burstAmount < 4 && burstUpdate <= Time.time)
        {
            burstUpdate = Time.time + burstInterval;
            TailAttack();
            burstAmount++;
        }
    }

    void BodyAttack()
    {
        Vector3 leftBodyBulletPos = gameObject.transform.GetChild(1).gameObject.transform.position;
        Vector3 rightBodyBulletPos = gameObject.transform.GetChild(2).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBodyBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBodyBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();

        if (player.transform.position.x <= transform.position.x)
        {
            bullet1.targetVector = (player.transform.position - transform.position).normalized;
        }
        else
        {
            bullet1.targetVector = new Vector3(-1, 0, 0);
        }
        if (player.transform.position.x >= transform.position.x)
        {
            bullet2.targetVector = (player.transform.position - transform.position).normalized;
        }
        else
        {
            bullet2.targetVector = new Vector3(-1, 0, 0);
        }

        bullet1.speed = 200;
        bullet2.speed = 200;

        bullet1.damage = 10;
        bullet2.damage = 10;
    }

    void TailAttack()
    {
        Vector3 leftTailBulletPos = gameObject.transform.GetChild(3).gameObject.transform.position;
        Vector3 rightTailBulletPos = gameObject.transform.GetChild(4).gameObject.transform.position;

        GameObject go3 = Instantiate(bulletPrefab, leftTailBulletPos, Quaternion.identity);
        GameObject go4 = Instantiate(bulletPrefab, rightTailBulletPos, Quaternion.identity);

        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();
        EnemyBullet bullet4 = go4.GetComponent<EnemyBullet>();

        bullet3.targetVector = (player.transform.position - transform.position).normalized;
        bullet4.targetVector = (player.transform.position - transform.position).normalized;

        bullet3.speed = 200;
        bullet4.speed = 200;

        bullet3.damage = 10;
        bullet4.damage = 10;
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
        go1.transform.localScale = new Vector3(3, 3, 1);
        game.notifyKill(points, "He111");

        FindObjectOfType<AudioManager>().Play("Explosion");
        FindObjectOfType<DialogueManager>().CreateEnemyDeathText(go1);
    }
}
