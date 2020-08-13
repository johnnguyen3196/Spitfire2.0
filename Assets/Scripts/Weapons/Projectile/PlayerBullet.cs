using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int speed;          // The speed our bullet travels
    public Vector3 targetVector;    // the direction it travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public int damage;       // how much damage this projectile causes
    public GameObject explosionPrefab;
    public Sprite upgradeBullet;

    // Start is called before the first frame update
    void Start()
    {
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        rb.AddForce(targetVector.normalized * speed);

        if(damage > 10f)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = upgradeBullet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // we have ran out of life
            Destroy(gameObject);    // kill me
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            if(enemy.TakeDamage(damage) <= 0)
            {
                enemy.Die();
            }
        }
    }
}
