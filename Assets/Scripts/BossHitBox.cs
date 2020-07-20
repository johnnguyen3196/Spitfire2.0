using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BossHitBox : MonoBehaviour
{
    private Boss boss;
    //assigned in Unity inspector. Represents wing or body hitbox
    public int component;
    public ParticleSystem flames;
    // Start is called before the first frame update
    void Start()
    {
        flames.Stop();
        boss = gameObject.transform.parent.GetComponent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerMissile")
        {
            PlayerMissile missile = collision.gameObject.GetComponent<PlayerMissile>();
            boss.TakeDamage(missile.damage, component);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>();
            boss.TakeDamage(bullet.damage, component);
            Destroy(collision.gameObject);
        }
    }
}
