using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class game : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;
    public GameObject enemyPrefab5;
    private int nextUpdate = 5;
    private int enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 5;
            UpdateEverySecond();
        }
    }

    void UpdateEverySecond()
    {
        enemy = Random.Range(1, 6);
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        switch (enemy)
        {
            case 1:
                GameObject go1 = Instantiate(enemyPrefab1, screenPosition, Quaternion.identity);
                EnemyPlane temp1 = go1.GetComponent<EnemyPlane>();
                temp1.type = 1;
                break;
            case 2:
                GameObject go2 = Instantiate(enemyPrefab2, screenPosition, Quaternion.identity);
                EnemyPlane temp2 = go2.GetComponent<EnemyPlane>();
                temp2.type = 2;
                break;
            case 3:
                GameObject go3 = Instantiate(enemyPrefab3, screenPosition, Quaternion.identity);
                EnemyPlane temp3 = go3.GetComponent<EnemyPlane>();
                temp3.type = 3;
                break;
            case 4:
                GameObject go4 = Instantiate(enemyPrefab4, screenPosition, Quaternion.identity);
                EnemyPlane temp4 = go4.GetComponent<EnemyPlane>();
                temp4.type = 4;
                break;
            case 5:
                GameObject go5 = Instantiate(enemyPrefab5, screenPosition, Quaternion.identity);
                EnemyPlane temp5 = go5.GetComponent<EnemyPlane>();
                temp5.type = 5;
                break;
        }
    }
}
