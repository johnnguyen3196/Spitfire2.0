using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squadron : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float RotateSpeed = 2f;
    private float Radius = 1f;

    public float angle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += RotateSpeed * Time.deltaTime;

        Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * Radius;
        transform.position = transform.parent.position + offset;
    }

    public void Attack()
    {
        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
        GameObject go = Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        PlayerBullet bullet = go.GetComponent<PlayerBullet>();
        bullet.targetVector = new Vector3(0, 1, 0);
        bullet.speed = 200;
        bullet.damage = 10;
    }
}
