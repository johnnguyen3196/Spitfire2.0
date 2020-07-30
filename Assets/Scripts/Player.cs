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
    public Sprite Spitfire;
    public Sprite Mustang;

    public int level;
    public string saveName;
    public int plane;

    private int oneSecondUpdate = 1;

    private float shootTimer = 0;
    private float defaultShootUpdate = 1;
    private float shootUpdate;
    private float missileTimer = 0;
    private float defaultMissileUpdate = 2;
    private float missileUpdate = 2;
    private float defaultSpeed = 5;
    public float speed;

    public GameObject missileTarget;

    public float maxHealth = 100;
    public float currentHealth;
    private float defaultMaxShieldHealth = 25;
    public float maxShieldHealth;
    public float currentShieldHealth;
    public int shootType;
    public int missileType;
    public int stance;

    public HealthBar healthBar;
    public TeleportBar teleportBar;
    public ShieldBar shieldBar;
    public StanceUI stanceUI;
    public Menu Menu;

    private float teleCoolDown = 0f;
    private float maxTeleCoolDown;
    private float defaultMaxTeleCoolDown = 5f;
    private float shieldCoolDown = 0f;
    private float maxShieldCoolDown;
    private float defaultMaxShieldCoolDown = 10f;

    private GameObject[] squadrons = new GameObject[0];

    public GameObject bulletPrefab;
    public GameObject powerUpPrefab;
    public GameObject playerMissilePrefab;
    public GameObject missileCrosshairPrefab;
    public GameObject squadronPrefab;
    
    public bool disableLeft;
    public bool disableRight;
    public bool disableUp;
    public bool disableDown;
    private Vector3 bottomLeft;
    private Vector3 topRight;

    //manual missile targeting on 1 sec cooldown
    private bool manualTarget;

    public ParticleSystem teleportDust;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        level = data.level;
        saveName = data.saveName;

        switch (data.plane)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = Spitfire;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = Mustang;
                break;
        }

        shootType = 0;
        missileType = 0;

        stance = 1;
        SetStance(stance);
        stanceUI.SelectAgility();

        currentHealth = maxHealth;
        currentShieldHealth = maxShieldHealth;
        healthBar.SetMaxHealth(maxHealth);
        shieldBar.SetMaxShield(maxShieldHealth);

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0 ,0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        manualTarget = false;

        SetNumberOfSquadrons(1);

        //temporary
        GameObject test = Instantiate(powerUpPrefab, new Vector3(0, 3, 0), Quaternion.identity);
        PowerUpScript powerUp = test.GetComponent<PowerUpScript>();
        powerUp.type = 9003;
    }

    // Update is called once per frame
    void Update()
    {
        //update every second
        if(Time.time >= oneSecondUpdate)
        {
            oneSecondUpdate = Mathf.FloorToInt(Time.time) + 1;
            manualTarget = false;
        }

        //Shooting updates
        if (Time.time >= shootTimer)
        {
            shootTimer = Time.time + shootUpdate;
            ShootGun();
            ShootSquadronGun();
        }

        if(Time.time >= missileTimer)
        {
            missileTimer = Time.time + missileUpdate;
            ShootMissile();
        }

        //Manual missile targeting updates
        if (Input.GetKey("left shift") && Input.GetMouseButton(0) && !manualTarget)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Boss")
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
            manualTarget = true;
        }

        //Teleport updates
        if (Input.GetKeyDown("z") && teleCoolDown == 0)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            teleCoolDown = maxTeleCoolDown;
            CreateDust();
        }
        if(teleCoolDown > 0)
        {
            teleCoolDown -= Time.deltaTime;
        } else
        {
            teleCoolDown = 0;
        }

        //Shield updates
        if(shieldCoolDown > 0)
        {
            shieldCoolDown -= Time.deltaTime;
        } else
        {
            if(currentShieldHealth != maxShieldHealth)
            {
                shieldCoolDown = 0;
                //Recover 5 shield/second
                currentShieldHealth += 5 * Time.deltaTime;
                //edge case
                if(currentShieldHealth > maxShieldHealth)
                {
                    currentShieldHealth = maxShieldHealth;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(stance != 1)
            {
                stance = 1;
                SetStance(stance);
                stanceUI.SelectAgility();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (stance != 2)
            {
                stance = 2;
                SetStance(stance);
                stanceUI.SelectGun();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (stance != 3)
            {
                stance = 3;
                SetStance(stance);
                stanceUI.SelectMissile();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (stance != 4)
            {
                stance = 4;
                SetStance(stance);
                stanceUI.SelectShield();
            }
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
        shieldBar.SetShield(currentShieldHealth);
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
        FindObjectOfType<AudioManager>().Play("Shoot");
    }

    void ShootSquadronGun()
    {
        for(int i = 0; i < squadrons.Length; i++)
        {

            Squadron squadron = squadrons[i].GetComponent<Squadron>();
            squadron.Attack();
        }
    }

    public void SetNumberOfSquadrons(int number)
    {
        //Reset current squadron
        for(int i = 0; i < squadrons.Length; i++)
        {
            Destroy(squadrons[i]);
        }

        //Spawn number of planes around player
        squadrons = new GameObject[number];
        float angleOffset = (360 / number) * Mathf.PI / 180;
        float angle = 0;
        for(int i = 0; i < number; i++)
        {
            Vector3 squadronPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            squadrons[i] = Instantiate(squadronPrefab, transform.position + squadronPos, Quaternion.identity);
            squadrons[i].transform.parent = transform;
            Squadron squadron = squadrons[i].GetComponent<Squadron>();
            squadron.angle = angle;
            angle += angleOffset;
        }
    }

    void ShootMissile()
    {
        switch (missileType)
        {
            case 0:
                SingleMissile();
                break;
            case 1:
                DoubleMissile();
                break;
            case 2:
                TripleMissile();
                break;
        }
        FindObjectOfType<AudioManager>().Play("Missile");
    }

    public void Die()
    {
        Menu.GameOverMenu("Wow, you suck");
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
    }

    void SingleMissile()
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        GameObject go = Instantiate(playerMissilePrefab, middleMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
    }

    void DoubleMissile()
    {
        Vector3 leftMissilePos = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
        Vector3 rightMissilePos = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
        GameObject go1 = Instantiate(playerMissilePrefab, leftMissilePos, Quaternion.identity);
        GameObject go2 = Instantiate(playerMissilePrefab, rightMissilePos, Quaternion.identity);
        PlayerMissile playerMissile1 = go1.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile2 = go2.GetComponent<PlayerMissile>();
        playerMissile1.damage = 10;
        playerMissile2.damage = 10;
    }

    void TripleMissile()
    {
        Vector3 middleMissilePos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Vector3 leftMissilePos = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
        Vector3 rightMissilePos = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
        GameObject go = Instantiate(playerMissilePrefab, middleMissilePos, Quaternion.identity);
        GameObject go1 = Instantiate(playerMissilePrefab, leftMissilePos, Quaternion.identity);
        GameObject go2 = Instantiate(playerMissilePrefab, rightMissilePos, Quaternion.identity);
        PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile1 = go1.GetComponent<PlayerMissile>();
        PlayerMissile playerMissile2 = go2.GetComponent<PlayerMissile>();
        playerMissile.damage = 10;
        playerMissile1.damage = 10;
        playerMissile2.damage = 10;
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

    public void TakeDamage(float damage)
    {
        float leftover = damage;
        if (currentShieldHealth > 0)
        {
            if(currentShieldHealth - damage < 0)
            {
                leftover = Math.Abs(currentShieldHealth - damage);
                currentShieldHealth = 0;
            } else
            {
                currentShieldHealth -= leftover;
                leftover = 0;
            }
            //taking damage on shield resets recharge cooldown
            shieldCoolDown = maxShieldCoolDown;

            FindObjectOfType<AudioManager>().Play("ShieldDamage");
        }
        if(leftover > 0)
        {
            Menu.TakeDamage();
            currentHealth -= leftover;
            FindObjectOfType<AudioManager>().Play("PlayerDamage");
        }
    }

    private void SetStance(int stance)
    {
        SetDefaultStats();
        switch (stance)
        {
            case 1:
                SetAgilityStance();
                break;
            case 2:
                SetGunStance();
                break;
            case 3:
                SetMissileStance();
                break;
            case 4:
                SetShieldStance();
                break;
        }
    }

    private void SetDefaultStats()
    {
        speed = defaultSpeed;
        maxTeleCoolDown = defaultMaxTeleCoolDown;
        maxShieldHealth = defaultMaxShieldHealth;
        maxShieldCoolDown = defaultMaxShieldCoolDown;
        shieldBar.SetMaxValue(maxShieldHealth);
        shootUpdate = defaultShootUpdate;
        missileUpdate = defaultMissileUpdate;
    }

    //speed increased by 25%
    //teleport cooldown lowered by 25%
    private void SetAgilityStance()
    {
        speed += defaultSpeed * .25f;
        maxTeleCoolDown -= defaultMaxTeleCoolDown * .25f;
    }

    //gun attackspeed increased by 25%
    private void SetGunStance()
    {
        shootUpdate -= defaultShootUpdate * .25f;
    }

    //missile attackspeed increased by 50%
    private void SetMissileStance()
    {
        missileUpdate -= defaultMissileUpdate * .5f;
    }

    //shield HP increased by 100%
    //shield recharge delay decreased by 50%
    private void SetShieldStance()
    {
        maxShieldHealth = defaultMaxShieldHealth * 2;
        maxShieldCoolDown -= defaultMaxTeleCoolDown * .5f;
        shieldBar.SetMaxValue(maxShieldHealth);
    }
}
