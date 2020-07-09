using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlane4 : MonoBehaviour
{
    private int nextUpdate = 5;
    public GameObject missilePrefab;
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
            nextUpdate = Mathf.FloorToInt(Time.time) + 5;
            UpdateEverySecond();
        }
    }

    private void UpdateEverySecond()
    {
        
    }
}
