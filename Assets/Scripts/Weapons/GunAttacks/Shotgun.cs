using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : PlayerAttack
{
    public GameObject bulletPrefab;

    public Shotgun()
    {
        //The name of your UI Sprite from Step 2
        this.UISpriteName = "Shotgun";
        //Set the type of your Upgrade. Either Type.Gun or Type.Missile
        this.type = Type.Gun;
        //Unique id for this upgrade. Check the wiki for a list of upgrades to make sure this id is unique.
        this.id = 10;
        //Load the PlayerBullet Prefab from the Resources folder. Check other upgrades as an example to see how you can use different projectiles
        bulletPrefab = Resources.Load("PlayerBulletObject") as GameObject;
    }

    public override void Attack(Transform transform)
    {
        //Shoot 3 bullets in a 6 degree arc
        float angle = (87.0f / 180.0f) * Mathf.PI;
        float offset = (3.0f / 180.0f) * Mathf.PI;

        for(int i = 0; i < 3; i++)
        {
            //Instantiate the PlayerBullet a little bit ahead of Player
            GameObject go = GameObject.Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z), Quaternion.identity);
            //Get the instantiate PlayerBullet class from GameObject
            PlayerBullet bullet = go.GetComponent<PlayerBullet>();
            //Set the direction the bullet will travel in
            bullet.targetVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            //Set the damage of the bullet
            bullet.damage = 10;
            //Set the speed of the bullet
            bullet.speed = 200;
            angle += offset;
        }
    }
}
