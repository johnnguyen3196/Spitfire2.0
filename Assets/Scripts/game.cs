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
    public GameObject cloudPrefab;
    private int enemyUpdate;
    private int cloudUpdate;
    private int enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemyUpdate = Random.Range(3, 6);
        cloudUpdate = Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //spawn cloud at random time (0 - 2 seconds)
        if(Time.time >= cloudUpdate) {
            int random = Random.Range(0, 3);
            cloudUpdate = Mathf.FloorToInt(Time.time) + random;
            UpdateCloudEverySecond();
        }

        //spawn enemy at random time (2 - 4 seconds)
        if (Time.time >= enemyUpdate)
        {
            int random = Random.Range(2, 5);
            enemyUpdate = Mathf.FloorToInt(Time.time) + random;
            UpdateEnemyEverySecond();
        }
    }

    void UpdateCloudEverySecond()
    {
        Vector3 cloudPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        GameObject go1 = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
    }

    void UpdateEnemyEverySecond()
    {
        //random enemy type and position
        enemy = Random.Range(1, 6);
        Vector3 enemyPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        enemyPosition.z = 0;
        switch (enemy)
        {
            case 1:
                GameObject go1 = Instantiate(enemyPrefab1, enemyPosition, Quaternion.identity);
                EnemyPlane temp1 = go1.GetComponent<EnemyPlane>();
                temp1.type = 1;
                break;
            case 2:
                GameObject go2 = Instantiate(enemyPrefab2, enemyPosition, Quaternion.identity);
                EnemyPlane temp2 = go2.GetComponent<EnemyPlane>();
                temp2.type = 2;
                break;
            case 3:
                GameObject go3 = Instantiate(enemyPrefab3, enemyPosition, Quaternion.identity);
                EnemyPlane temp3 = go3.GetComponent<EnemyPlane>();
                temp3.type = 3;
                break;
            case 4:
                GameObject go4 = Instantiate(enemyPrefab4, enemyPosition, Quaternion.identity);
                EnemyPlane temp4 = go4.GetComponent<EnemyPlane>();
                temp4.type = 4;
                break;
            case 5:
                GameObject go5 = Instantiate(enemyPrefab5, enemyPosition, Quaternion.identity);
                EnemyPlane temp5 = go5.GetComponent<EnemyPlane>();
                temp5.type = 5;
                break;
        }
    }
}
