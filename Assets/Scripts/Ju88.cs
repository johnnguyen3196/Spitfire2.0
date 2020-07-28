using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ju88 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;

    public GameObject bombPrefab;
    public GameObject explosionPrefab;
    private game game;

    private float speed = 1f;
    private int attackSpeed = 3;
    private int currentHealth = 100;
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

        points = 40;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
    }

    void Attack()
    {
        GameObject go = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Bomb bomb = go.GetComponent<Bomb>();
        bomb.targetVector = new Vector3(0, -1, 0);
        bomb.lifetime = 3f;
        bomb.speed = 2f;
        // spawn 6 - 10 projectiles
        bomb.numberOfBullets = Random.Range(6, 11);
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
        game.notifyKill(points);
    }
}
