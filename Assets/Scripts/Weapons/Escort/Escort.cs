using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escort : MonoBehaviour
{
    public float RotateSpeed;
    public float Radius;
    public float AttackSpeed;

    public float angle;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        angle += RotateSpeed * Time.deltaTime;

        Vector3 offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * Radius;
        transform.position = transform.parent.position + offset;
    }

    public virtual void Attack()
    {

    }
}
