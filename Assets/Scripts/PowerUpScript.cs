using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PowerUpScript : MonoBehaviour
{
    private int speed = 100;
    //TODO random or assignable type
    public int type;
    public ShootTypeUI shootTypeMenu;
    public ShootTypeUI missileTypeMenu;
    public ShootTypeUI squadronTypeUI;

    public Sprite tripleShotSprite;
    public Sprite quadShotSprite;
    public Sprite upgradeTripleShotSprite;
    public Sprite doubleMissileSprite;
    public Sprite tripleMissileSprite;
    public Sprite doubleSquadronSprite;
    public Sprite tripleSquadronSprite;
    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = tripleShotSprite;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = quadShotSprite;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = upgradeTripleShotSprite;
                break;
            case -1:
                gameObject.GetComponent<SpriteRenderer>().sprite = doubleMissileSprite;
                break;
            case -2:
                gameObject.GetComponent<SpriteRenderer>().sprite = tripleMissileSprite;
                break;
            case 9002:
                gameObject.GetComponent<SpriteRenderer>().sprite = doubleSquadronSprite;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case 9003:
                gameObject.GetComponent<SpriteRenderer>().sprite = tripleSquadronSprite;
                transform.localScale = new Vector3(1, 1, 1);
                break;

        }
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        Vector3 targetVector = new Vector3(0, -1, 0);
        rb.AddForce(targetVector.normalized * speed);
        shootTypeMenu = GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>();
        missileTypeMenu = GameObject.Find("MissileTypeUI").GetComponent<ShootTypeUI>();
        squadronTypeUI = GameObject.Find("SquadronTypeUI").GetComponent<ShootTypeUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "plane")
        {
            Destroy(gameObject);
            Player player = collision.gameObject.GetComponent<Player>();
            Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            if (type > 0)
            {
                //squadron powerup
                if(type > 9000)
                {
                    player.SetNumberOfSquadrons(type - 9000);
                    squadronTypeUI.ChangeShootSprite(sprite);
                } else
                {
                    //gun powerup
                    player.shootType = type;
                    shootTypeMenu.ChangeShootSprite(sprite);
                }
            } else
            {
                //missile powerup
                player.missileType = Mathf.Abs(type);
                missileTypeMenu.ChangeShootSprite(sprite);
            }
        }
    }
}
