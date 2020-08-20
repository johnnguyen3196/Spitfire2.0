using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabOnExit : MonoBehaviour
{
    float lifetime = 10f;
    Vector3 bottom;
    void Start()
    {
        lifetime += Time.time;
        bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
    }

    void Update()
    {
        if(Time.time > lifetime)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
