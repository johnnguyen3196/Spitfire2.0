using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 targetVector;
    public float speed;
    public int numberOfBullets;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = targetVector * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= transform.position.y)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        float angleOffset = (360 / numberOfBullets) * Mathf.PI / 180;
        float angle = 0;
        for (int i = 0; i < numberOfBullets; i++, angle += angleOffset)
        {
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            PlayerBullet bullet = go.GetComponent<PlayerBullet>();
            bullet.targetVector = direction;
            bullet.speed = 150;
            bullet.damage = 10;
        }
    }
}
