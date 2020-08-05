using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

//TODO Move circle collider to player object
public class PlayerMissile : MonoBehaviour
{
    public float speed;          // The speed our bullet travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public int damage;
    public Vector3 targetVector;
    public GameObject target = null;
    private Player player;
    private Rigidbody2D rb;
    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("plane").GetComponent<Player>();
        speed = 100f;
        //missile will initially go straight until 0.5 seconds
        if(targetVector == null)
        {
            targetVector = new Vector3(0, 1, 0);
        }
        rb.AddForce(targetVector * speed);
        spawnTime = Time.time;
        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(target != null)
        {
            speed = 5f;
            rb.velocity = (target.transform.position - transform.position).normalized * speed;
            transform.right = target.transform.position - transform.position;
        }
        else
        {
            target = player.GetMissileTarget();
        }
        
        if(spawnTime + lifetime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            
            EnemyInterface enemy = collision.gameObject.GetComponent<EnemyInterface>();
            if (enemy.TakeDamage(damage) <= 0)
            {
                enemy.Die();
            }
        }
    }
}
