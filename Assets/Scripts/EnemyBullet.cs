using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int speed;          // The speed our bullet travels
    public Vector3 targetVector;    // the direction it travels
    public float damage;       // how much damage this projectile causes
    public GameObject explosionPrefab;
    public Sprite upgradeBullet;

    // Start is called before the first frame update
    void Start()
    {
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        rb.AddForce(targetVector.normalized * speed);
        if (damage > 10f)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = upgradeBullet;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "plane")
        {
            Destroy(gameObject);
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            if (player.currentHealth <= 0)
            {
                player.Die();
            }
        }
    }
}
