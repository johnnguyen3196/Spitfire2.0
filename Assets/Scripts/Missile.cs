using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 1f;          // The speed our bullet travels
    public Vector3 targetVector;    // the direction it travels
    public float lifetime = 10f;     // how long it lives before destroying itself
    public float damage = 10;       // how much damage this projectile causes
    public GameObject target;   // the target i want to get closer to 
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.Find("plane");
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce((target.transform.position - transform.position).normalized * speed);
        Vector3 moveDirection = gameObject.transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
