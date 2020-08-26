using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Escort
{
    public int damage;
    // Update is called once per frame
    public override void Update()
    {
        angle += RotateSpeed * Time.deltaTime;

        Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * Radius;
        transform.position = transform.parent.position + offset;

        transform.right = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyPlane enemy = collision.gameObject.GetComponent<EnemyPlane>();
            if (enemy.TakeDamage(damage) <= 0)
            {
                enemy.Die();
            }
        }

        if(collision.gameObject.tag == "EnemyProjectile")
        {
            //Destroy a projectile 50% of the time
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
