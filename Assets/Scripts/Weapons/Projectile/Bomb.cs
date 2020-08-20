using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float spawnTime;
    public float lifetime;
    public GameObject bulletPrefab;
    public int numberOfBullets;
    public Vector3 targetVector;
    public float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = targetVector * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= spawnTime + lifetime)
        {
            Explode();
            Destroy(gameObject);
        }
    }
    
    void Explode()
    {
        float angleOffset = (360 / numberOfBullets) * Mathf.PI / 180;
        float angle = 0;
        for(int i = 0; i < numberOfBullets; i++, angle += angleOffset)
        {
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            EnemyBullet bullet = go.GetComponent<EnemyBullet>();
            bullet.targetVector = direction;
            bullet.speed = 150;
            bullet.damage = 10;
        }
    }
}
