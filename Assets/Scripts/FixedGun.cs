using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FixedGun : MonoBehaviour, BossWeaponInterface
{
    public GameObject bulletPrefab;

    private GameObject player;

    private float attackSpeed;
    private float attackUpdate = 0;
    private int attackPattern;
    private int burstIndex;
    private int numberOfShots;

    private bool newAttack = true;

    private float angleOffset;
    private float currentAngle;

    private bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("plane");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > attackUpdate && !disabled)
        {
            if (newAttack)
            {
                SetAttackPattern();
                burstIndex = 0;
                newAttack = false;
            }
            Attack();
        }
    }

    void SetAttackPattern()
    {
        attackPattern = Random.Range(0, 3);
        switch (attackPattern)
        {
            case 0:
                numberOfShots = 5;
                attackSpeed = 2;
                break;
            case 1:
                numberOfShots = Random.Range(5, 11);
                currentAngle = Mathf.PI;
                angleOffset = Mathf.PI / numberOfShots;
                attackSpeed = .75f;
                break;
            case 2:
                numberOfShots = Random.Range(3, 6);
                attackSpeed = .25f;
                break;
        }
    }

    void Attack()
    {
        //shot enough times, set a new attack pattern
        if(burstIndex > numberOfShots)
        {
            newAttack = true;
        } else
        {
            switch (attackPattern)
            {
                case 0:
                    AttackPattern0();
                    break;
                case 1:
                    AttackPattern1();
                    break;
                case 2:
                    AttackPattern2();
                    break;
            }
            attackUpdate = Time.time + attackSpeed;
        }
    }

    //Simply shoot down every 2 seconds 5 times
    void AttackPattern0()
    {
        Vector3 leftBulletPos = gameObject.transform.GetChild(0).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();

        bullet1.targetVector = new Vector3(0, -1, 0);

        bullet1.speed = 150;
        bullet1.damage = 10;
    }
    //Shoot 5 - 10 times in a 180 degree angle towards player
    void AttackPattern1()
    {
        Vector3 leftBulletPos = gameObject.transform.GetChild(0).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();

        Vector3 direction = new Vector3(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle), 0);
        bullet1.targetVector = direction;
        currentAngle += angleOffset;

        bullet1.speed = 150;
        bullet1.damage = 10;
    }

    //burst fire 3 - 5 shots towards the player
    void AttackPattern2()
    {
        Vector3 leftBulletPos = gameObject.transform.GetChild(0).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();

        bullet1.targetVector = (player.transform.position - transform.position).normalized;

        bullet1.speed = 150;
        bullet1.damage = 10;
    }

    public void DisableWeapon()
    {
        disabled = true;
    }
}
