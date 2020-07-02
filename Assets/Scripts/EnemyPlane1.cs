using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyPlane1 : MonoBehaviour
{
    private int nextUpdate = 2;
    public GameObject bulletPrefab;
    public int speed = 50;
    public Vector3 targetVector;
    // Start is called before the first frame update
    void Start()
    {
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
            nextUpdate = Mathf.FloorToInt(Time.time) + 2;
            UpdateEverySecond();
        }
    }

    private void UpdateEverySecond()
    {
        Vector3 leftBulletPos = new Vector3(transform.position.x - 0.233f, transform.position.y - .65f, transform.position.z);
        Vector3 rightBulletPos = new Vector3(transform.position.x + 0.233f, transform.position.y - .65f, transform.position.z);
        GameObject go1 = Instantiate(bulletPrefab, leftBulletPos, Quaternion.identity);
        GameObject go2 = Instantiate(bulletPrefab, rightBulletPos, Quaternion.identity);
        Bullet bullet1 = go1.GetComponent<Bullet>();
        Bullet bullet2 = go2.GetComponent<Bullet>();
        bullet1.targetVector = new Vector3(0, -1, 0);
        bullet2.targetVector = new Vector3(0, -1, 0);
    }
}
