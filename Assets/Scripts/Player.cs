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
    public int escortType;
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
    public GameObject playerMissilePrefab;
    public GameObject missileCrosshairPrefab;
    public GameObject squadronPrefab;

    //temp
    public GameObject powerUpPrefab;
    
    public bool disableLeft;
    public bool disableRight;
    public bool disableUp;
    public bool disableDown;
    private Vector3 bottomLeft;
    private Vector3 topRight;

    //manual missile targeting on 1 sec cooldown
    private bool manualTarget;

    public ParticleSystem teleportDust;

    private PlayerAttack gunAttack;
    private PlayerAttack missileAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
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
        shootType = data.shootType;
        missileType = data.missileType;
        SetNumberOfSquadrons(data.escortType);

        SetGunType(8);
        SetMissileType(missileType);

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

        //temporary
        //GameObject test = Instantiate(powerUpPrefab);
        //PowerUpScript powerUp = test.GetComponent<PowerUpScript>();
        //powerUp.type = 4;

        FindObjectOfType<DialogueManager>().Create(gameObject, "Hello World");
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
        gunAttack.Attack(transform);
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
        escortType = number;
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
        GameObject.Find("SquadronTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIStandardEscort" + number.ToString()));
    }

    void ShootMissile()
    {
        missileAttack.Attack(transform);
        FindObjectOfType<AudioManager>().Play("Missile");
    }

    private void SetGunType(int shootType)
    {
        switch (shootType)
        {
            case 0:
                gunAttack = new DoubleShot(bulletPrefab);
                //Change the Gun sprite in UI
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIDoubleShot"));
                break;
            case 1:
                gunAttack = new TripleShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UITripleShot"));
                break;
            case 2:
                gunAttack = new QuadShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIQuadShot"));
                break;
            case 3:
                gunAttack = new UpgradedTripleShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIUpgradedTripleShot"));
                break;
            case 4:
                gunAttack = new AutoCannon(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIAutoCannon"));
                shootUpdate = .25f;
                defaultShootUpdate = .25f;
                break;
            case 5:
                gunAttack = new HighVelocityShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIHighVelocityShot"));
                break;
            case 6:
                gunAttack = new UpgradedDoubleShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIUpgradedDoubleShot"));
                break;
            case 7:
                gunAttack = new DoubleAutoCannon(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIDoubleAutoCannon"));
                break;
            case 8:
                gunAttack = new SmartHighVelocityShot(bulletPrefab);
                GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UISmartHighVelocityShot"));
                break;
        }
    }

    private void SetMissileType(int missileType)
    {
        switch (missileType)
        {
            case 1:
                missileAttack = new SingleMissile(playerMissilePrefab);
                GameObject.Find("MissileTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UISingleMissile"));
                break;
            case 2:
                missileAttack = new DoubleMissile(playerMissilePrefab);
                GameObject.Find("MissileTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIDoubleMissile"));
                break;
            case 3:
                missileAttack = new TripleMissile(playerMissilePrefab);
                GameObject.Find("MissileTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UITripleMissile"));
                break;
            case 4:
                missileAttack = new SwarmerMissile(playerMissilePrefab);
                GameObject.Find("MissileTypeUI").GetComponent<ShootTypeUI>().ChangeShootSprite(FindObjectOfType<UISpriteManager>().Find("UIQuadMissile"));
                break;
        }
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

    public void Die()
    {
        GameObject.Find("Game").GetComponent<game>().notifyKill(-1, "");
        //Menu.GameOverMenu("Wow, you suck");
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
