using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PowerUpScript : MonoBehaviour
{
    private int speed = 100;
    //TODO random or assignable type
    public int type;
    public ShootTypeUI Menu;
    public Sprite tripleShotSprite;
    public Sprite quadShotSprite;
    public Sprite upgradeTripleShotSprite;
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
        }
        // find our RigidBody
        Rigidbody2D rb = gameObject.GetComponentInChildren<Rigidbody2D>();
        // add force 
        Vector3 targetVector = new Vector3(0, -1, 0);
        rb.AddForce(targetVector.normalized * speed);
        Menu = GameObject.Find("ShootTypeUI").GetComponent<ShootTypeUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "plane")
        {
            Destroy(gameObject);
            Player player = collision.gameObject.GetComponent<Player>();
            player.shootType = type;
            Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            Menu.ChangeShootSprite(sprite);
        }
    }
}
