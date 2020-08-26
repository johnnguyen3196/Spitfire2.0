using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwarmerMissile : PlayerAttack
{
    public GameObject missilePrefab;
    public SwarmerMissile()
    {
        this.UISpriteName = "UISwarmerMissile";
        this.type = Type.Missile;
        this.id = 4;
    }

    public override void Attack(Transform transform)
    {
        if (missilePrefab == null)
        {
            missilePrefab = Resources.Load("PlayerMissileObject") as GameObject;
        }

        Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(1, 1, 0), new Vector3(-1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0) };
        foreach (Vector3 direction in directions)
        {
            GameObject go = GameObject.Instantiate(missilePrefab, transform.position, Quaternion.identity);
            PlayerMissile playerMissile = go.GetComponent<PlayerMissile>();
            playerMissile.damage = 5;
            playerMissile.targetVector = direction;
            go.transform.localScale = new Vector3(1.5f, 1.5f, 0);
        }
    }
}
