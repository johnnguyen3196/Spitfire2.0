using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW_190 : EnemyPlane
{
    public GameObject bulletPrefab;

    public override void InitializeEnemy()
    {
        this.rb.velocity = new Vector3(0, -1, 0) * speed;
    }

    public override void Attack()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - .65f, transform.position.z);

        //Shoot 5 - 8 bullets
        int numberOfBullets = Random.Range(5, 9);

        //shoot down
        float angle = (255.0f / 180.0f) * Mathf.PI;
        //shoot in spread of 30 degrees
        float angleOffset = (Mathf.PI / 6) / numberOfBullets;
        for (int i = 0; i < numberOfBullets + 1; i++)
        {
            //create the EnemyBullet GameObject at pos.
            GameObject go = Instantiate(bulletPrefab, pos, Quaternion.identity);

            //Get the EnemyBullet script from GameObject
            EnemyBullet bullet = go.GetComponent<EnemyBullet>();

            //Set the direction the bullet travels in
            bullet.targetVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            //Set the speed of the bullet
            bullet.speed = 150;

            //Set the damage of the bullet
            bullet.damage = 10;

            angle += angleOffset;
        }
    }
}
