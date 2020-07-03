using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 200;          // The speed our bullet travels
    public Vector3 targetVector;    // the direction it travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public float damage = 10;       // how much damage this projectile causes
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        rb.AddForce(targetVector.normalized * speed);
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
        if (collision.gameObject.name.Equals("EnemyObject1(Clone)") || collision.gameObject.name.Equals("EnemyObject1") || collision.gameObject.name.Equals("EnemyObject4(Clone)") || collision.gameObject.name.Equals("EnemyObject4"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GameObject go1 = Instantiate(explosionPrefab, collision.gameObject.transform.position, Quaternion.identity);
        }
    }
}
