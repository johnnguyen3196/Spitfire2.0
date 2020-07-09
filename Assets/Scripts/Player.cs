using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    private int nextUpdate = 1;
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public PauseMenu PauseMenu;
    private string[] targetNames = new string[] { "EnemyObject1(Clone)", "EnemyObject1", "EnemyObject2(Clone)", "EnemyObject2", "EnemyObject3(Clone)", "EnemyObject3", "EnemyObject4(Clone)", "EnemyObject4", "EnemyObject5(Clone)", "EnemyObject5"};
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //GameObject test = Instantiate(enemyPrefab, new Vector3(0, 3, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
            }
        }
        healthBar.SetHealth(currentHealth);
    }

    void UpdateEverySecond()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet1.targetNames = targetNames;
        bullet2.targetNames = targetNames;
    }

    public void Die()
    {
        PauseMenu.GameOverMenu();
    }
}
