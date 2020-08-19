using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabOnExit : MonoBehaviour
{
    Vector3 bottom;
    void Start()
    {
        bottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
    }

    void OnBecameInvisible()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
