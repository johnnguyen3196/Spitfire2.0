using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the poison field GameObject that is created by a poisen missile
public class Poisonfield : MonoBehaviour
{
    public float lifetime;
    private float nextUpdate = 0;
    private float starttime;
    //Tracks all enemies that are in the GameObject
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

        //Damage all enemies inside the poison
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + 1;
            DamageAllEnemies();
        }
    }

    void DamageAllEnemies()
    {
        // Deal damgage to all enemies in Enemies array
        foreach(GameObject enemyObject in enemies.ToArray())
        {
            EnemyPlane enemy = enemyObject.GetComponent<EnemyPlane>();
            if (enemy.TakeDamage(10) <= 0)
            {
                enemy.Die();
            }
        }
    }

    //If an enemy enters the GameObject, add to Enemies array and slow down enemy
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject);
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            enemy.ModifySpeed(-.5f);
        }
    }

    //Enemy exits GameObject, remove from Enemies array and set speed to default
    void OnTriggerExit2D(Collider2D collision)
    {
        //An enemy exits the GameObject, remove from Enemies array
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
