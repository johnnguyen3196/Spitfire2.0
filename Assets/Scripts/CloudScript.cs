using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public Sprite[] clouds;
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(40, 76);
        //get a random cloud
        int random = Random.Range(0, clouds.Length);
        this.GetComponent<SpriteRenderer>().sprite = clouds[random];
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        Vector3 targetVector = new Vector3(0, -1, 0);
        rb.AddForce(targetVector.normalized * speed);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
