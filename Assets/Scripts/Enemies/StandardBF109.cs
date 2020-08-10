using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBF109 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    private game game;

    private float speed = 2.5f;
    private int attackSpeed = 2;
    public int currentHealth;
    private Vector3 targetVector;
    private int points;

    private Animation anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, -1, 0) * speed;

        game = GameObject.Find("Game").GetComponent<game>();

        anim = gameObject.GetComponent<Animation>();

        points = 10;
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
        game.notifyKill(points, "BF109E");

        FindObjectOfType<AudioManager>().Play("Explosion");
        FindObjectOfType<DialogueManager>().CreateEnemyDeathText(go1);
    }
}
