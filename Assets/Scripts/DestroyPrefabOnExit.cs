using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabOnExit : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
