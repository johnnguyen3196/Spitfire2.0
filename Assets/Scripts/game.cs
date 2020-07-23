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
    public GameObject ju88Prefab;
    public GameObject cloudPrefab;
    public PointsUI pointsUI;
    public Menu Menu;

    private int enemyUpdate;
    private int cloudUpdate;

    private int enemy;
    private int[] enemyTypes;
    private int[] enemyPoints = new int[] {0, 10, 20, 50, 30, 30, 50, 20, 30, 40};
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
            //enemyTypes[i] = Random.Range(1, 10);
            enemyTypes[i] = 9;
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
                break;
            case 2:
                GameObject go2 = Instantiate(enemyPrefab2, enemyPosition, Quaternion.identity);
                break;
            case 3:
                GameObject go3 = Instantiate(enemyPrefab3, enemyPosition, Quaternion.identity);
                break;
            case 4:
                GameObject go4 = Instantiate(enemyPrefab4, enemyPosition, Quaternion.identity);
                break;
            case 5:
                GameObject go5 = Instantiate(enemyPrefab5, enemyPosition, Quaternion.identity);
                break;
            case 6:
                GameObject go6 = Instantiate(enemyPrefab6, enemyPosition, Quaternion.identity);
                break;
            case 7:
                GameObject go7 = Instantiate(enemyPrefab7, enemyPosition, Quaternion.identity);
                break;
            case 8:
                GameObject go8 = Instantiate(enemyPrefab8, enemyPosition, Quaternion.identity);
                break;
            case 9:
                GameObject go9 = Instantiate(ju88Prefab, enemyPosition, Quaternion.identity);
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
