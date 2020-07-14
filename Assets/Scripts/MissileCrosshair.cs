using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCrosshair : MonoBehaviour
{
    private float rotateUpdate = .1f;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= rotateUpdate)
        {
            rotateUpdate += Time.time + rotateUpdate;
            gameObject.transform.rotation = Quaternion.AngleAxis(1, Vector3.forward);
        }
    }
}
