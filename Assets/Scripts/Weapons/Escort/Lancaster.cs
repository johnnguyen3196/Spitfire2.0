using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancaster : Escort
{
    // This GameObject will be assigned through the Unity Inspector and Instantiated in the Attack() function.
    public GameObject bombPrefab;

    public override void Attack()
    {
        // Instantiate the PlayerBomb prefab
        GameObject go = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        //Get the PlayerBomb script from the GameObject we just instantiated
        PlayerBomb bomb = go.GetComponent<PlayerBomb>();

        //Shoot the bomb up
        bomb.targetVector = new Vector3(0, 1, 0);

        //Set the speed of the bomb
        bomb.speed = 2;

        //The bomb will detonate and produce 5-9 PlayerBullet projectiles
        bomb.numberOfBullets = Random.Range(5, 10);
    }
}
