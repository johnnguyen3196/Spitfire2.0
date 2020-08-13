using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ju88 : EnemyPlane
{
    public GameObject bombPrefab;

    public override void InitializeEnemy()
    {
        rb.velocity = new Vector3(0, -1, 0) * speed;
    }

    public override void Attack()
    {
        GameObject go = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Bomb bomb = go.GetComponent<Bomb>();
        bomb.targetVector = new Vector3(0, -1, 0);
        bomb.lifetime = 2f;
        bomb.speed = 2f;
        // spawn 6 - 10 projectiles
        bomb.numberOfBullets = Random.Range(6, 11);
    }
}
