using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 50f;          // The speed our bullet travels
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
        targetVector = gameObject.transform.position;
        if (targetVector != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
