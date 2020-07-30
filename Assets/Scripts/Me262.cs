using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Me262 : MonoBehaviour, EnemyInterface
{
    private int nextUpdate = 0;

    public GameObject missilePrefab;
    public GameObject explosionPrefab;
    private game game;

    private float speed = 3.5f;
    private int attackSpeed = 3;
    public int currentHealth;
    private Vector3 targetVector;
    private int points;

    private GameObject player;

    private Rigidbody2D rb;
    private Animation anim;

    private bool rotate;
    //false left, true right
    private bool rotateDirection;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        anim = gameObject.GetComponent<Animation>();

        game = GameObject.Find("Game").GetComponent<game>();

        player = GameObject.Find("plane");

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        rotate = true;
        if (transform.position.x > 0)
        {
            rotateDirection = false;
        }
        else
        {
            rotateDirection = true;
        }

        points = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
        Move();
    }

    void Attack()
    {
        Vector3 missilePos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        GameObject go1 = Instantiate(missilePrefab, missilePos, Quaternion.identity);
        Missile missile = go1.GetComponent<Missile>();
        missile.targetVector = new Vector3(0, -1, 0);
    }

    private void Move()
    {
        if (rotate)
        {
            //left
            if (!rotateDirection)
            {
                //stop left rotation past 210 degrees
                if (transform.eulerAngles.z < 195)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z - .3f, Vector3.forward);
                }
            }
            else
            {
                //right
                //stop right rotation past 330 degrees
                if (transform.eulerAngles.z > 345)
                {
                    rotate = false;
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + .3f, Vector3.forward);
                }
            }
        }
        if (transform.position.x < bottomLeft.x + 2)
        {
            rotate = true;
            rotateDirection = true;
        }
        if (transform.position.x > topRight.x - 2)
        {
            rotate = true;
            rotateDirection = false;
        }
        rb.velocity = transform.right.normalized * speed;
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
