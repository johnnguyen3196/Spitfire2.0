using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMissile : PlayerMissile
{
    public GameObject poisonfield;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Instantiate(poisonfield, transform.position, Quaternion.identity);
        }
    }
}
