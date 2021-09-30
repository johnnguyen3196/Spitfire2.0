using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Escort
{
    private Rigidbody2D rb;

    private GameObject player;
    public Vector3 playerPosition;
    public Vector3 mousePosition;

    public int damage;
    public float thrustSpeed;

    public bool thrust;

    public Vector3 velocity;

    public override void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("plane");
        thrust = true;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        transform.parent = null;
    }

    public override void Update()
    {
        if (thrust)
        {    
            rb.velocity = (mousePosition - transform.position).normalized * thrustSpeed;
            transform.right = mousePosition - transform.position;
        } else
        {
            playerPosition = player.transform.position;
            rb.velocity = (playerPosition - transform.position).normalized * thrustSpeed;
            transform.right = transform.position - playerPosition;
        }
        velocity = rb.velocity;
        Switch();
    }

    private void Switch()
    {
        if (thrust)
        {
            Vector3 difference = mousePosition - transform.position;
            difference = VectorAbsolute(difference);
            if (difference.x < 0.3 && difference.y < 0.3)
            {
                thrust = false;
            }
        } else
        {
            Vector3 difference = playerPosition - transform.position;
            difference = VectorAbsolute(difference);
            if (difference.x < 0.3 && difference.y < 0.3)
            {
                thrust = true;
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
            }
        }
    } 

    private Vector3 VectorAbsolute(Vector3 vector)
    {
        Vector3 returnVector;
        returnVector.x = Mathf.Abs(vector.x);
        returnVector.y = Mathf.Abs(vector.y);
        returnVector.z = Mathf.Abs(vector.z);
        return returnVector;
    }

    private  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            if (enemy.TakeDamage(damage) <= 0)
            {
                enemy.Die();
            }
        }

        if (collision.gameObject.tag == "EnemyProjectile")
        {
            //Destroy a projectile 50% of the time
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
