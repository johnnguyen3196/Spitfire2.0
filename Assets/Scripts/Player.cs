using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    private int shootUpdate = 1;
    private int missileUpdate = 2;
    public float speed;

    private GameObject missileTarget;

    public int maxHealth = 100;
    public int currentHealth;
    public int shootType;

    public HealthBar healthBar;
    public TeleportBar teleportBar;
    public Menu Menu;

    private float teleCoolDown = 0f;

    public GameObject bulletPrefab;
    public GameObject powerUpPrefab;
    public GameObject playerMissilePrefab;
    public GameObject missileCrosshairPrefab;
    
    public bool disableLeft;
    public bool disableRight;
    public bool disableUp;
    public bool disableDown;
    private Vector3 bottomLeft;
    private Vector3 topRight;

    private string[] targetNames = new string[] { "EnemyObject1(Clone)", "EnemyObject1", "EnemyObject2(Clone)", "EnemyObject2", "EnemyObject3(Clone)", "EnemyObject3", "EnemyObject4(Clone)", "EnemyObject4", "EnemyObject5(Clone)", "EnemyObject5"};

    public ParticleSystem teleportDust;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        shootType = 0;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        GameObject test = Instantiate(powerUpPrefab, new Vector3(0, 3, 0), Quaternion.identity);
        PowerUpScript powerUp = test.GetComponent<PowerUpScript>();
        powerUp.type = 3;
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0 ,0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        //Shooting updates
        if (Time.time >= shootUpdate)
        {
            shootUpdate = Mathf.FloorToInt(Time.time) + 1;
            ShootGun();
        }

        if(Time.time >= missileUpdate)
        {
            missileUpdate = Mathf.FloorToInt(Time.time) + 2;
            ShootMissile();
        }

        //Manual missile targeting updates
        if (Input.GetKey("left shift") && Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if(hit.collider.name.Substring(0, 5) == "Enemy")
                {
                    //destroy previous crosshair
                    GameObject[] temp = GameObject.FindGameObjectsWithTag("MissileCrosshair");
                    if(temp.Length > 0)
                    {
                        Destroy(temp[0]);
                    }
                    missileTarget = hit.transform.gameObject;
                    SetMissileTarget(missileTarget);
                }
            }
        }

        //Teleport updates
        if (Input.GetKeyDown("z") && teleCoolDown == 0)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            teleCoolDown = 5f;
            CreateDust();
        }
        if(teleCoolDown > 0)
        {
            teleCoolDown -= Time.deltaTime;
        } else
        {
            teleCoolDown = 0;
        }

        //Movement updates
        float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.Translate(movement);
        float xPosition = transform.position.x;
        float yPosition = transform.position.y;
        if (xPosition < bottomLeft.x)
        {
            xPosition = bottomLeft.x;
        }
        if (xPosition > topRight.x)
        {
            xPosition = topRight.x;
        }
        if (yPosition < bottomLeft.y)
        {
            yPosition = bottomLeft.y;
        }
        if (yPosition > topRight.y)
        {
            yPosition = topRight.y;
        }
        transform.position = new Vector3(xPosition, yPosition, 0);

        //UI updates
        healthBar.SetHealth(currentHealth);
        teleportBar.SetTele(1 - Mathf.Lerp(0, 1, teleCoolDown / 5));
    }

    void ShootGun()
    {
        switch (shootType)
        {
            case 0:
                DoubleShot();
                break;
            case 1:
                TripleShot();
                break;
            case 2:
                QuadShot();
                break;
            case 3:
                UpgradeTripleShot();
                break;
        }
    }

    void ShootMissile()
    {
        FireMissile();
    }

    public void Die()
    {
        Menu.GameOverMenu();
    }

    //Default shooting type
    void DoubleShot()
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

    void TripleShot()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        PlayerBullet bullet3 = go3.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet3.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet3.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
        bullet1.targetNames = targetNames;
        bullet2.targetNames = targetNames;
        bullet3.targetNames = targetNames;
    }

    void QuadShot()
    {
        Vector3 leftLeftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightRightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 middleLeftBulletPos = new Vector3(transform.position.x - .1f, transform.position.y + .4f, transform.position.z);
        Vector3 middleRightBulletPos = new Vector3(transform.position.x + .1f, transform.position.y + .4f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftLeftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightRightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleLeftBulletPos, Quaternion.identity);
        GameObject go4 = Instantiate(bulletPrefab, middleRightBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        PlayerBullet bullet3 = go3.GetComponent<PlayerBullet>();
        PlayerBullet bullet4 = go4.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet3.targetVector = new Vector3(0, 1, 0);
        bullet4.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet3.speed = 200;
        bullet4.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 10;
        bullet4.damage = 10;
        bullet1.targetNames = targetNames;
        bullet2.targetNames = targetNames;
        bullet3.targetNames = targetNames;
        bullet4.targetNames = targetNames;
    }

    void UpgradeTripleShot()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y + 0.371f, transform.position.z);
        Vector3 middleBulletPos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        GameObject go3 = Instantiate(bulletPrefab, middleBulletPos, Quaternion.identity);
        PlayerBullet bullet1 = go1.GetComponent<PlayerBullet>();
        PlayerBullet bullet2 = go2.GetComponent<PlayerBullet>();
        PlayerBullet bullet3 = go3.GetComponent<PlayerBullet>();
        bullet1.targetVector = new Vector3(0, 1, 0);
        bullet2.targetVector = new Vector3(0, 1, 0);
        bullet3.targetVector = new Vector3(0, 1, 0);
        bullet1.speed = 200;
        bullet2.speed = 200;
        bullet3.speed = 200;
        bullet1.damage = 10;
        bullet2.damage = 10;
        bullet3.damage = 30;
        bullet1.targetNames = targetNames;
        bullet2.targetNames = targetNames;
        bullet3.targetNames = targetNames;
    }

    void FireMissile()
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = Instantiate(playerMissilePrefab, middleMissilePos, Quaternion.identity);
    }

    void CreateDust()
    {
        teleportDust.Play();
    }

    public GameObject GetMissileTarget()
    {
        if(missileTarget == null)
        {
            missileTarget = GameObject.FindGameObjectWithTag("Enemy");
            if(missileTarget != null)
            {
                Debug.Log("Adding crosshair to " + missileTarget.name);
                GameObject go = Instantiate(missileCrosshairPrefab, missileTarget.transform.position, Quaternion.identity);
                MissileCrosshair missileCrosshair = go.GetComponent<MissileCrosshair>();
                missileCrosshair.target = missileTarget;
            }
        }
        return missileTarget;
    }

    public void ResetMissileTarget()
    {
        missileTarget = null;
        foreach(GameObject missileObject in GameObject.FindGameObjectsWithTag("PlayerMissile"))
        {
            PlayerMissile missile = missileObject.GetComponent<PlayerMissile>();
            missile.target = null;
        }
    }

    public void SetMissileTarget(GameObject target)
    {
        //add targeting crosshair if enemy doesn't have one yet
        if (GameObject.FindGameObjectsWithTag("MissileCrosshair").Length == 0)
        {
            GameObject go = Instantiate(missileCrosshairPrefab, missileTarget.transform.position, Quaternion.identity);
            MissileCrosshair missileCrosshair = go.GetComponent<MissileCrosshair>();
            missileCrosshair.target = missileTarget;
        }
        foreach (GameObject missileObject in GameObject.FindGameObjectsWithTag("PlayerMissile"))
        {
            PlayerMissile missile = missileObject.GetComponent<PlayerMissile>();
            missile.target = target;
        }
    }
}
