using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject UIPrefab;

    //Originally, the turrets were the child of each wing and body component. But for some stupid reason, the turrets scaled differently and looked really weird when rotated.
    //Now they are the child of the boss prefab itself. STUPID
    public GameObject[] bodyTurrets;
    public GameObject leftWingTurret;
    public GameObject rightWingTurret;

    private float speed = .5f;
    public int maxHealth = 1000;
    //component 0
    private int bodyHealth = 600;
    //component 1
    private int leftWingHealth = 300;
    //component 2
    private int rightWingHealth = 300;
    public int currentHealth;

    public HealthBar healthBar;
    private GameObject healthBarUI;

    private Rigidbody2D rb;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    private game game;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(1, 0, 0) * speed;

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        currentHealth = maxHealth;

        Player player = GameObject.Find("plane").GetComponent<Player>();
        player.missileTarget = gameObject;

        game = GameObject.Find("Game").GetComponent<game>();

        healthBarUI = Instantiate(UIPrefab);
        healthBar = healthBarUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < bottomLeft.x + 2)
        {
            rb.velocity = new Vector3(1, 0, 0) * speed;
        }
        if (transform.position.x > topRight.x - 2)
        {
            rb.velocity = new Vector3(-1, 0, 0) * speed;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage, int component)
    {
        switch (component)
        {
            case 0:
                if(bodyHealth > 0)
                {
                    bodyHealth -= damage;
                    currentHealth -= damage;
                    if(bodyHealth <= 0)
                    {
                        GameObject child = gameObject.transform.GetChild(3).gameObject;
                        BossHitBox hitbox = child.GetComponent<BossHitBox>();
                        hitbox.flames.Play();

                        foreach(GameObject go in bodyTurrets)
                        {
                            BossWeaponInterface weapon = go.GetComponent<BossWeaponInterface>();
                            weapon.DisableWeapon();
                        }
                        DisableComponentWeapons(child);
                    }
                }
                break;
            case 1:
                if (leftWingHealth > 0)
                {
                    leftWingHealth -= damage;
                    currentHealth -= damage;
                    if (leftWingHealth <= 0)
                    {
                        GameObject child = gameObject.transform.GetChild(1).gameObject;
                        BossHitBox hitbox = child.GetComponent<BossHitBox>();
                        hitbox.flames.Play();

                        BossWeaponInterface weapon = leftWingTurret.GetComponent<BossWeaponInterface>();
                        weapon.DisableWeapon();
                        DisableComponentWeapons(child);
                    }
                }
                break;
            case 2:
                if (rightWingHealth > 0)
                {
                    rightWingHealth -= damage;
                    currentHealth -= damage;
                    if (rightWingHealth <= 0)
                    {
                        GameObject child = gameObject.transform.GetChild(2).gameObject;
                        BossHitBox hitbox = child.GetComponent<BossHitBox>();
                        hitbox.flames.Play();

                        BossWeaponInterface weapon = rightWingTurret.GetComponent<BossWeaponInterface>();
                        weapon.DisableWeapon();
                        DisableComponentWeapons(child);
                    }
                }
                break;
        }
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            Destroy(healthBarUI);
            GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //BIG BOOM
            go.transform.localScale = new Vector3(7, 7, 1);
            game.notifyKill(9001, "BV228");
        }
    }

    void DisableComponentWeapons(GameObject component)
    {
        foreach(Transform child in component.transform)
        {
            BossWeaponInterface weapon = child.gameObject.GetComponent<BossWeaponInterface>();
            weapon.DisableWeapon();
        }
    }
}
