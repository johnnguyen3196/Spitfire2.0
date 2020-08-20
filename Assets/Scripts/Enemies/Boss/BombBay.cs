using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombBay : MonoBehaviour, BossWeaponInterface
{
    public GameObject bombPrefab;

    private float attackUpdate = 0;

    private bool disabled = false;
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= attackUpdate && !disabled)
        {
            attackUpdate = Mathf.FloorToInt(Time.time) + Random.Range(4, 6);
            Attack();
        }
    }

    void Attack()
    {
        GameObject go = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Bomb bomb = go.GetComponent<Bomb>();
        bomb.targetVector = new Vector3(0, -1, 0);
        bomb.lifetime = 2f;
        bomb.speed = 2f;
        // spawn 6 - 10 projectiles
        bomb.numberOfBullets = Random.Range(6, 11);
    }

    public void DisableWeapon()
    {
        disabled = true;
    }
}
