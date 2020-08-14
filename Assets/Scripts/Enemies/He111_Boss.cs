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

    public override void InitializeEnemy() {
        rb.velocity = new Vector3(1, 0, 0) * speed;

        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        player = GameObject.Find("plane");

        game = GameObject.Find("Game").GetComponent<game>();

        healthBarUI = Instantiate(UIPrefab);
        healthBar = healthBarUI.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(currentHealth);
    }

    protected override void Update()
    {
        if(Time.time >= movementUpdate)
        {
            Move();
        }
    }

    public override void Attack()
    {

    }

    public override void Move()
    {
        Vector3 velocity = Vector3.zero;
        //move randomly left or right
        switch(Random.Range(0, 2))
        {
            case 0:
                velocity = moveLeft();
                break;
            case 1:
                velocity = moveRight();
                break;
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

            Debug.Log("Right " + velocity);
        }
        //boss too close to right, move left
        if (transform.position.x > topRight.x - 3)
        {
            velocity = moveLeft();
        }
        rb.velocity = velocity;
        movementUpdate += 1;
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
}
