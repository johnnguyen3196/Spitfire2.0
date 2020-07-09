using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyPlane : MonoBehaviour
{
    private int nextUpdate = 0;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public int speed;
    public int attackSpeed;
    public int currentHealth;
    public int type;
    public Vector3 targetVector;
    private string[] targetNames = new string[] {"plane"};
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case 1:
                speed = 50;
                attackSpeed = 2;
                currentHealth = 10;
                break;
            case 2:
                speed = 50;
                attackSpeed = 2;
                currentHealth = 20;
                break;
            case 3:
                speed = 75;
                attackSpeed = 2;
                currentHealth = 50;
                break;
            case 4:
                speed = 50;
                attackSpeed = 5;
                currentHealth = 30;
                break;
            case 5:
                speed = 100;
                attackSpeed = 2;
                currentHealth = 20;
                break;
        }

        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        targetVector = new Vector3(0, -1, 0);
        rb.AddForce(targetVector.normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
    }

    private void Attack()
    {
        switch (type)
        {
            case 1:
                Enemy1AttackPattern();
                break;
            case 2:
                Enemy2AttackPattern();
                break;
            case 3:
                Enemy3AttackPattern();
                break;
            case 4:
                Enemy4AttackPattern();
                break;
            case 5:
                Enemy5AttackPattern();
                break;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject go1 = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
    }

    private void Enemy1AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet1.speed = 100;
        bullet2.speed = 100;
        bullet1.damage = 10;
        bullet2.damage = 10;
    }

    private void Enemy2AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);
        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet3.targetVector = new Vector3(0, -1, 0);
        bullet1.speed = 100;
        bullet2.speed = 100;
        bullet3.speed = 100;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
    }

    private void Enemy3AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.15f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.15f, transform.position.y - .65f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y - .75f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);
        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        EnemyBullet bullet3 = go3.GetComponent<EnemyBullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet3.targetVector = new Vector3(0, -1, 0);
        bullet1.speed = 100;
        bullet2.speed = 100;
        bullet3.speed = 100;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
    }

    private void Enemy4AttackPattern()
    {
        Vector3 missilePos = new Vector3(transform.position.x, transform.position.y - .8f, transform.position.z);
        GameObject go1 = Instantiate(missilePrefab, missilePos, Quaternion.identity);
        Missile missile = go1.GetComponent<Missile>();
        missile.targetVector = new Vector3(0, -1, 0);
    }

    private void Enemy5AttackPattern()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();
        EnemyBullet bullet2 = go2.GetComponent<EnemyBullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
        bullet1.speed = 150;
        bullet2.speed = 150;
        bullet1.damage = 10;
        bullet2.damage = 10;
    }
}
