using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyPlane : MonoBehaviour
{
    protected int attackUpdate = 0;

    public GameObject explosionPrefab;

    protected game game;

    public string _name;
    public float speed;
    private float defaultSpeed;
    public int attackSpeed;
    public int currentHealth;
    protected Vector3 targetVector;
    public int points;

    private bool speedModified = false;

    protected Animation anim;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        InitializeDefaults();
        InitializeEnemy();
        defaultSpeed = speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time >= attackUpdate)
        {
            attackUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
           
            Attack();
        }
    }

    public virtual void InitializeEnemy()
    {

    }

    private void InitializeDefaults()
    {
        this.game = GameObject.Find("Game").GetComponent<game>();

        this.anim = gameObject.GetComponent<Animation>();

        this.rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void Attack()
    {

    }

    public virtual void Move()
    {
    }

    public virtual int TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.Play("EnemyDamageAnimation");
        return currentHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject go1 = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        game.notifyKill(points, _name);

        FindObjectOfType<AudioManager>().Play("Explosion");
        FindObjectOfType<DialogueManager>().CreateEnemyDeathText(go1);
    }

    public virtual void ModifySpeed(float percentage)
    {
        if (!speedModified)
        {
             
            speed += speed * percentage;

            if(targetVector == Vector3.zero)
            {
                rb.velocity = new Vector3(0, -1, 0) * speed;
            } else
            {
                Debug.Log(rb.velocity);
                rb.velocity = targetVector * speed;
                Debug.Log(rb.velocity);
            }
            speedModified = true;
        }
    }

    public virtual void SetDefaultSpeed()
    {
        speed = defaultSpeed;
        speedModified = false;
    }
}
