using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ju87 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private game game;
    private GameObject player;

    private float speed = 2f;
    private int attackSpeed = 1;
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

        points = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
    }

    void Attack()
    {
        //player is behind GameObject
        if (player.transform.position.y < transform.position.y)
        {
            attackSpeed = 1;
            GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            EnemyBullet bullet = go.GetComponent<EnemyBullet>();
            bullet.targetVector = (player.transform.position - transform.position).normalized;
            bullet.speed = 150;
            bullet.damage = 10;
        }
        else
        {
            //player is in front of GameObject
            attackSpeed = 2;

            Vector3 leftBulletPos = gameObject.transform.GetChild(1).gameObject.transform.position;
            Vector3 rightBulletPos = gameObject.transform.GetChild(2).gameObject.transform.position;

            GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
            GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);

            EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
            EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();

            bullet1.targetVector = new Vector3(0, 1, 0);
            bullet2.targetVector = new Vector3(0, 1, 0);

            bullet1.speed = 150;
            bullet2.speed = 150;
            bullet1.damage = 10;
            bullet2.damage = 10;
        }
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
        game.notifyKill(points);

        FindObjectOfType<AudioManager>().Play("Explosion");
    }
}
