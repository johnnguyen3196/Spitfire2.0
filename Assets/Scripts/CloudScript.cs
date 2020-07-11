using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public Sprite cloud1;
    public Sprite cloud2;
    public Sprite cloud3;
    public Sprite cloud4;
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(40, 76);
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().sprite = cloud1;
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = cloud2;
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = cloud3;
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = cloud4;
                break;
        }
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
