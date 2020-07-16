using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float nextUpdate = .5f;
    public float speed;          // The speed our bullet travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public float damage = 10;       // how much damage this projectile causes
    public GameObject target;   // the player 
    private Rigidbody2D rb;
    private float distance;     // distance between missile and player
    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.Find("plane");
        distance = Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position);
        speed = 100f;
        //missile will initially go straight until 0.5 seconds
        rb.AddForce(new Vector3(0, -1, 0) * speed);
        spawnTime = Time.time;
        transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        //missile starts tracking after 0.5 seconds
        if(Time.time >= spawnTime + nextUpdate)
        {
            speed = 1f;
            //missile will continue in current direction if it overshoots the player
            float currentDistance = Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position);
            if (currentDistance <= distance + .2f)
            {
                rb.AddForce((target.transform.position - transform.position).normalized * speed);
                transform.right = target.transform.position - transform.position;

                distance = currentDistance;
            }
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
