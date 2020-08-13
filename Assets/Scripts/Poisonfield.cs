using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisonfield : MonoBehaviour
{
    public float lifetime;
    private float nextUpdate = 0;
    private float starttime;
    private List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        starttime = Time.time;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= starttime + lifetime)
        {
            Destroy(gameObject);
        }

        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + .5f;
            DamageAllEnemies();
        }
    }

    void DamageAllEnemies()
    {
        foreach(GameObject enemyObject in enemies)
        {
            EnemyPlane enemy = enemyObject.GetComponent<EnemyPlane>();
            if (enemy.TakeDamage(10) <= 0)
            {
                enemy.Die();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject);
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            enemy.ModifySpeed(-.5f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int index = enemies.FindIndex(plane => plane.GetInstanceID() == collision.gameObject.GetInstanceID());
            if(index != -1)
            {
                enemies.RemoveAt(index);
            }
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            enemy.SetDefaultSpeed();
        }
    }
}
