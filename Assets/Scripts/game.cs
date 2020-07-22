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
    public GameObject enemyPrefab6;
    public GameObject enemyPrefab7;
    public GameObject enemyPrefab8;
    public GameObject cloudPrefab;
    public PointsUI pointsUI;
    public Menu Menu;

    private int enemyUpdate;
    private int cloudUpdate;

    private int enemy;
    private int[] enemyTypes;
    private int[] enemyPoints = new int[] {0, 10, 20, 50, 30, 30, 50, 30, 30};
    private int spawnIndex = 0;

    private bool spawnBoss = false;
    //edge case. Only spawn the boss once
    private bool bossSpawned = false;

    public int totalPoints = 0;
    public int currentPoints = 0;

    public GameObject bossPrefab;
    // Start is called before the first frame update
    void Start()
    {
        enemyUpdate = Random.Range(3, 6);
        cloudUpdate = Random.Range(0, 3); 

        //Spawn between 40-50 enemies
        int numberOfEnemies = Random.Range(40, 51);
        enemyTypes = new int[numberOfEnemies];
        for(int i = 0; i < numberOfEnemies; i++)
        {
            enemyTypes[i] = Random.Range(1, 9);
        }
        for(int i = 0; i < numberOfEnemies; i++)
        {
            totalPoints += enemyPoints[enemyTypes[i]];
        }
        //Player only needs to score only 80% of the enemies spawned
        totalPoints = Mathf.FloorToInt(totalPoints * .8f);
        pointsUI.setPointsText(0, totalPoints);
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

        //spawn enemy at random time(2 - 4 seconds)
        if (Time.time >= enemyUpdate && !bossSpawned)
        {
            int random = Random.Range(2, 5);
            enemyUpdate = Mathf.FloorToInt(Time.time) + random;
            UpdateEnemyEverySecond();
        }

        if (spawnBoss)
        {
            Instantiate(bossPrefab, new Vector3(0, 3, 0), Quaternion.identity);
            spawnBoss = false;
            bossSpawned = true;
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
        int enemy = enemyTypes[spawnIndex];
        spawnIndex++;
        Vector3 enemyPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        enemyPosition.z = 0;
        switch (enemy)
        {
            case 1:
                GameObject go1 = Instantiate(enemyPrefab1, enemyPosition, Quaternion.identity);
                EnemyPlane temp1 = go1.GetComponent<EnemyPlane>();
                temp1.type = 1;
                temp1.points = enemyPoints[enemy];
                break;
            case 2:
                GameObject go2 = Instantiate(enemyPrefab2, enemyPosition, Quaternion.identity);
                EnemyPlane temp2 = go2.GetComponent<EnemyPlane>();
                temp2.type = 2;
                temp2.points = enemyPoints[enemy];
                break;
            case 3:
                GameObject go3 = Instantiate(enemyPrefab3, enemyPosition, Quaternion.identity);
                EnemyPlane temp3 = go3.GetComponent<EnemyPlane>();
                temp3.type = 3;
                temp3.points = enemyPoints[enemy];
                break;
            case 4:
                GameObject go4 = Instantiate(enemyPrefab4, enemyPosition, Quaternion.identity);
                EnemyPlane temp4 = go4.GetComponent<EnemyPlane>();
                temp4.type = 4;
                temp4.points = enemyPoints[enemy];
                break;
            case 5:
                GameObject go5 = Instantiate(enemyPrefab5, enemyPosition, Quaternion.identity);
                EnemyPlane temp5 = go5.GetComponent<EnemyPlane>();
                temp5.type = 5;
                temp5.points = enemyPoints[enemy];
                break;
            case 6:
                GameObject go6 = Instantiate(enemyPrefab6, enemyPosition, Quaternion.identity);
                EnemyPlane temp6 = go6.GetComponent<EnemyPlane>();
                temp6.type = 6;
                temp6.points = enemyPoints[enemy];
                break;
            case 7:
                GameObject go7 = Instantiate(enemyPrefab7, enemyPosition, Quaternion.identity);
                EnemyPlane temp7 = go7.GetComponent<EnemyPlane>();
                temp7.type = 7;
                temp7.points = enemyPoints[enemy];
                break;
            case 8:
                GameObject go8 = Instantiate(enemyPrefab8, enemyPosition, Quaternion.identity);
                EnemyPlane temp8 = go8.GetComponent<EnemyPlane>();
                temp8.type = 8;
                temp8.points = enemyPoints[enemy];
                break;
        }
    }

    public void notifyKill(int points)
    {
        //ITS OVER 9000!!!!
        if(points > 9000)
        {
            Menu.GameOverMenu("Mission Complete");
            return;
        }
        currentPoints += points;
        if(currentPoints >= totalPoints && !bossSpawned)
        {
            spawnBoss = true;
        }
        pointsUI.setPointsText(currentPoints, totalPoints);
    }
}
