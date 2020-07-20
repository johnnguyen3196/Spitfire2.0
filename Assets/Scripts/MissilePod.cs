using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissilePod : MonoBehaviour
{
    public GameObject missilePrefab;

    public GameObject player;

    private int attackSpeed;
    private float attackUpdate;
    private Vector3[] directions = {new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0)};

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("plane");
        attackUpdate = Time.time + Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > attackUpdate)
        {
            attackSpeed = Random.Range(0, 6);
            attackUpdate = Mathf.FloorToInt(Time.time) + attackSpeed;
            Attack();
        }
    }

    void Attack()
    {
        int numberOfMissiles = Random.Range(1, 5);
        for(int i = 0; i < numberOfMissiles; i++)
        {
            GameObject go = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            Missile missile = go.GetComponent<Missile>();
            missile.targetVector = directions[i];
        }
    }
}
