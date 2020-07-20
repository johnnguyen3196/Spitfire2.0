using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject UIPrefab;

    private float speed = .5f;
    public int maxHealth = 1000;
    //component 0
    private int bodyHealth = 600;
    //component 1
    private int leftWingHealth = 300;
    //component 2
    private int rightWingHealth = 300;
    public int currentHealth;

    public HealthBar healthBar;
    private GameObject healthBarUI;

    private float attackSpeed;
    private float attackUpdate = 0;

    private Rigidbody2D rb;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(1, 0, 0) * speed;

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        currentHealth = maxHealth;

        Player player = GameObject.Find("plane").GetComponent<Player>();
        player.missileTarget = gameObject;

        healthBarUI = Instantiate(UIPrefab);
        healthBar = healthBarUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > attackUpdate)
        {
            Attack();
            //randomly attack between 1.5 to 3 seconds
            attackSpeed = Random.Range(150, 300) / 100;
            attackUpdate = Time.time + attackSpeed;
        }
        if (transform.position.x < bottomLeft.x + 2)
        {
            rb.velocity = new Vector3(1, 0, 0) * speed;
        }
        if (transform.position.x > topRight.x - 2)
        {
            rb.velocity = new Vector3(-1, 0, 0) * speed;
        }
        healthBar.SetHealth(currentHealth);
    }

    void Attack()
    {
        Vector3 bullet1Pos = gameObject.transform.GetChild(1).gameObject.transform.position;
        Vector3 bullet2Pos = gameObject.transform.GetChild(2).gameObject.transform.position;
        Vector3 bullet3Pos = gameObject.transform.GetChild(3).gameObject.transform.position;
        Vector3 bullet4Pos = gameObject.transform.GetChild(4).gameObject.transform.position;
        Vector3 bullet5Pos = gameObject.transform.GetChild(5).gameObject.transform.position;
        Vector3 bullet6Pos = gameObject.transform.GetChild(6).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, bullet1Pos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, bullet2Pos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, bullet3Pos, Quaternion.identity);
        GameObject go4 = Instantiate(bulletPrefab, bullet4Pos, Quaternion.identity);
        GameObject go5 = Instantiate(bulletPrefab, bullet5Pos, Quaternion.identity);
        GameObject go6 = Instantiate(bulletPrefab, bullet6Pos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();
        EnemyBullet bullet4 = go4.GetComponent<EnemyBullet>();
        EnemyBullet bullet5 = go5.GetComponent<EnemyBullet>();
        EnemyBullet bullet6 = go6.GetComponent<EnemyBullet>();

        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet3.targetVector = new Vector3(0, -1, 0);
        bullet4.targetVector = new Vector3(0, -1, 0);
        bullet5.targetVector = new Vector3(0, -1, 0);
        bullet6.targetVector = new Vector3(0, -1, 0);

        bullet1.speed = 150;
        bullet2.speed = 150;
        bullet3.speed = 150;
        bullet4.speed = 150;
        bullet5.speed = 150;
        bullet6.speed = 150;

        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
        bullet4.damage = 10;
        bullet5.damage = 10;
        bullet6.damage = 10;
    }

    public void TakeDamage(int damage, int component)
    {
        switch (component)
        {
            case 0:
                if(bodyHealth > 0)
                {
                    bodyHealth -= damage;
                    currentHealth -= damage;
                    if(bodyHealth <= 0)
                    {
                        BossHitBox hitbox = gameObject.transform.GetChild(17).gameObject.GetComponent<BossHitBox>();
                        hitbox.flames.Play();
                    }
                }
                break;
            case 1:
                if (leftWingHealth > 0)
                {
                    leftWingHealth -= damage;
                    currentHealth -= damage;
                    if (leftWingHealth <= 0)
                    {
                        BossHitBox hitbox = gameObject.transform.GetChild(15).gameObject.GetComponent<BossHitBox>();
                        hitbox.flames.Play();
                    }
                }
                break;
            case 2:
                if (rightWingHealth > 0)
                {
                    rightWingHealth -= damage;
                    currentHealth -= damage;
                    if (rightWingHealth <= 0)
                    {
                        BossHitBox hitbox = gameObject.transform.GetChild(16).gameObject.GetComponent<BossHitBox>();
                        hitbox.flames.Play();
                    }
                }
                break;
        }
        if(currentHealth < 0)
        {
            Destroy(gameObject);
            Destroy(healthBarUI);
            GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //BIG BOOM
            go.transform.localScale = new Vector3(7, 7, 1);
        }
    }
}
