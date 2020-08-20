using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BossHitBox : EnemyPlane
{
    private Boss boss;
    //assigned in Unity inspector. Represents wing or body hitbox
    public int component;
    public ParticleSystem flames;
    // Start is called before the first frame update
    public override void InitializeEnemy()
    {
        flames.Stop();
        boss = gameObject.transform.parent.GetComponent<Boss>();
    }

    public override int TakeDamage(int damage)
    {
        boss.TakeDamage(damage, component);
        //Return something > 0 since the boss will handle the dieing, not the projectile
        return 1;
    }

    public override void ModifySpeed(float percentage)
    {
    }

    public override void SetDefaultSpeed()
    {
    }
}
