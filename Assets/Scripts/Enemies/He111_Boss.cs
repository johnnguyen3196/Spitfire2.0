using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class He111_Boss : EnemyPlane
{
    private float movementUpdate;

    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public GameObject UIPrefab;

    private GameObject player;

    public HealthBar healthBar;
    private GameObject healthBarUI;

    private Vector3 bottomLeft;
    private Vector3 topRight;

    public Vector3[] downDirections;
    public Vector3[] leftDirections;
    public Vector3[] rightDirections;
    public Vector3[] upDirections;

    private float bombUpdate;

    //Body Attacks
    private int leftNumberOfShots = 0;
    private int rightNumberOfShots = 0;

    private int leftBodyAttackPattern;
    private int rightBodyAttackPattern;

    private float leftBodyAttackUpdate = 0;
    private float rightBodyAttackUpdate = 0;

    private float leftBodyAttackSpeed;
    private float rightBodyAttackSpeed;

    private float leftBodyAttackAngle;
    private float rightBodyAttackAngle;

    private float leftBodyAngleOffset;
    private float rightBodyAngleOffset;

    //false: left, true: right
    private bool direction;

    public override void InitializeEnemy() {
        rb.velocity = new Vector3(1, 0, 0) * speed;

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        player = GameObject.Find("plane");

        game = GameObject.Find("Game").GetComponent<game>();

        healthBarUI = Instantiate(UIPrefab);
        healthBar = healthBarUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(currentHealth);
        healthBar.SetName("He111H");

        //movement direction randomly left or right
        direction = Random.value > 0.5f ? true : false;
    }

    protected override void Update()
    {
        float time = Time.time;
        if (time >= movementUpdate)
        {
            Move();
            movementUpdate = time + 1;
        }
        if(time >= bombUpdate)
        {
            BombAttack();
            bombUpdate = time + 1;
        }
        if (time >= leftBodyAttackUpdate)
        {
            LeftBodyAttack();
            leftBodyAttackUpdate = time + leftBodyAttackSpeed;
            leftNumberOfShots--;
        }
        if (time >= rightBodyAttackUpdate)
        {
            RightBodyAttack();
            rightBodyAttackUpdate = time + rightBodyAttackSpeed;
            rightNumberOfShots--;
        }
    }

    private void BombAttack()
    {
        GameObject go = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        Bomb bomb = go.GetComponent<Bomb>();
        bomb.targetVector = new Vector3(0, -1, 0);
        bomb.lifetime = 1.5f;
        bomb.speed = 1.5f;
        // spawn 6 - 10 projectiles
        bomb.numberOfBullets = Random.Range(6, 11);
    }

    private void chooseAttackPattern(int direction)
    {
        if(direction < 0)
        {
            if (player == null)
                return;
            if (player.transform.position.x > transform.position.x)
            {
                int attackPattern = Random.Range(0, 5);
                if(attackPattern > 1)
                {
                    leftBodyAttackPattern = 2;
                } else
                {
                    leftBodyAttackPattern = attackPattern;
                }
                
            } else
            {
                leftBodyAttackPattern = Random.Range(0, 2);
            }
            switch (leftBodyAttackPattern)
            {
                case 0:
                    leftNumberOfShots = Random.Range(6, 11);
                    leftBodyAttackSpeed = 0.5f;
                    leftBodyAttackAngle = Mathf.PI / 2;
                    leftBodyAngleOffset = Mathf.PI / leftNumberOfShots - 1;
                    break;
                case 1:
                    leftNumberOfShots = Random.Range(3, 6);
                    leftBodyAttackSpeed = 1;
                    break;
                case 2:
                    leftNumberOfShots = Random.Range(5, 11);
                    leftBodyAttackSpeed = 0.5f;
                    break;
            }
        } else
        {
            if (player == null)
                return;
            if (player.transform.position.x > transform.position.x)
            {
                int attackPattern = Random.Range(0, 5);
                if (attackPattern > 1)
                {
                    rightBodyAttackPattern = 2;
                }
                else
                {
                    rightBodyAttackPattern = attackPattern;
                }

            }
            else
            {
                rightBodyAttackPattern = Random.Range(0, 2);
            }
            switch (rightBodyAttackPattern)
            {
                case 0:
                    rightNumberOfShots = Random.Range(6, 11);
                    rightBodyAttackSpeed = 0.5f;
                    rightBodyAttackAngle = Mathf.PI / 2;
                    rightBodyAngleOffset = Mathf.PI / rightNumberOfShots - 1;
                    break;
                case 1:
                    rightNumberOfShots = Random.Range(3, 6);
                    rightBodyAttackSpeed = 1;
                    break;
                case 2:
                    rightNumberOfShots = Random.Range(5, 11);
                    rightBodyAttackSpeed = 0.5f;
                    break;
            }
        }
    }

    private void LeftBodyAttack()
    {
        if(leftNumberOfShots <= 0)
        {
            chooseAttackPattern(-1);
        }
        Vector3 pos = transform.GetChild(0).transform.position;
        switch (leftBodyAttackPattern)
        {
            case 0:
                leftBodyAttackAngle = AttackPattern0(-1, pos, leftBodyAttackAngle, leftBodyAngleOffset);
                break;
            case 1:
                AttackPattern1(-1, pos);
                break;
            case 2:
                AttackPattern2(-1);
                break;
        }
    }

    private void RightBodyAttack()
    {
        if (rightNumberOfShots <= 0)
        {
            chooseAttackPattern(1);
        }
        Vector3 pos = transform.GetChild(1).transform.position;
        switch (rightBodyAttackPattern)
        {
            case 0:
                rightBodyAttackAngle = AttackPattern0(1, pos, rightBodyAttackAngle, rightBodyAngleOffset);
                break;
            case 1:
                AttackPattern1(1, pos);
                break;
            case 2:
                AttackPattern2(1);
                break;
        }
    }

    private float AttackPattern0(int direction, Vector3 pos, float currentAngle, float angleOffset)
    {
        GameObject go1 = Instantiate(bulletPrefab, pos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();

        Vector3 angle = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0);
        bullet1.targetVector = angle;

        bullet1.speed = 150;
        bullet1.damage = 10;

        return currentAngle += angleOffset * (-direction);
    }

    public void AttackPattern1(int direction, Vector3 pos)
    {
        int numberOfShots = Random.Range(4, 7);
        float angleOffset = (45 * Mathf.PI / 180) / numberOfShots * (-direction);
        float currentAngle = direction > 0 ? (22.5f * Mathf.PI / 180) : (157.5f * Mathf.PI / 180);
        for(int i = 0; i < numberOfShots + 1; i++)
        {
            GameObject go = Instantiate(bulletPrefab, pos, Quaternion.identity);
            EnemyBullet bullet = go.GetComponent<EnemyBullet>();
            Vector3 angle = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0);
            bullet.targetVector = angle;
            currentAngle += angleOffset;

            bullet.speed = 150;
            bullet.damage = 10;
        }
    }

    void AttackPattern2(int direction)
    {
        if (player == null)
            return;
        //if this is the right gun, and the player is on the left side of boss, stop shooting
        if(direction > 0 && player.transform.position.x < transform.position.x)
        {
            rightNumberOfShots = 0;
            return;
        }
        //if this is the left gun, and the player is on the right side of boss, stop shooting
        if (direction < 0 && player.transform.position.x > transform.position.x)
        {
            leftNumberOfShots = 0;
            return;
        }
        Vector3 leftBulletPos = gameObject.transform.GetChild(0).gameObject.transform.position;

        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);

        EnemyBullet bullet1 = go1.GetComponent<EnemyBullet>();

        bullet1.targetVector = (player.transform.position - transform.position).normalized;

        bullet1.speed = 150;
        bullet1.damage = 10;
    }

    public override void Move()
    {
        Vector3 velocity = Vector3.zero;
        //change directino 10% of the time
        if(Random.Range(0, 11) == 0)
        {
            if (direction)
            {
                velocity = moveLeft();
                direction = false;
            } else
            {
                velocity = moveRight();
                direction = true;
            }
        } else
        {
            if (direction)
            {
                velocity = moveRight();
            } else
            {
                velocity = moveLeft();
            }
        }

        //boss too close to top, move down
        if (transform.position.y > topRight.y - 3)
        {
            velocity = moveDown();
        }
        //boss too close to left, move right
        if(transform.position.x < bottomLeft.x + 3)
        {
            velocity = moveRight();
            direction = true;
        }
        //boss too close to right, move left
        if (transform.position.x > topRight.x - 3)
        {
            velocity = moveLeft();
            direction = false;
        }
        //boss to close to bottom, move up
        if (transform.position.y < 0)
        {
            velocity = moveUp();
        }
        rb.velocity = velocity;
    }

    private Vector3 moveDown()
    {
        return downDirections[Random.Range(0, downDirections.Length)] * speed;
    }

    private Vector3 moveLeft()
    {
        return leftDirections[Random.Range(0, leftDirections.Length)] * speed;
    }

    private Vector3 moveRight()
    {
        return rightDirections[Random.Range(0, rightDirections.Length)] * speed;
    }

    private Vector3 moveUp()
    {
        return upDirections[Random.Range(0, upDirections.Length)] * speed;
    }

    private Vector3 moveRandom()
    {
        Vector3 velocity = Vector3.zero;
        switch (Random.Range(0, 2))
        {
            case 0:
                velocity = moveLeft();
                break;
            case 1:
                velocity = moveRight();
                break;
        }
        return velocity * speed;
    }

    public override int TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.Play("EnemyDamageAnimation");

        healthBar.SetHealth(currentHealth);

        return currentHealth;
    }
}
