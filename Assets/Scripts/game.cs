using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;


/* Usage: Use in every Level created in Unity by attatching this script to an empty GameObject
 * Purpose: Instantiate EnemyPlanes, instantiate a boss at the end, manage number of points player has.
 * 
 */
public class game : MonoBehaviour
{
    #region Variables set in Unity Inspector
    public GameObject cloudPrefab;

    public GameObject bossPrefab;

    public PointsUI pointsUI;
    public Menu Menu;
    public Scoreboard scoreboard;

    public int totalPoints;
    public int requiredPoints;
    public int currentPoints = 0;

    public int level;
 
    #endregion

    private float enemyUpdate;
    private int cloudUpdate;

    private float bossSpawnTime = 0;

    private bool endGame = false;
    private string endMessage;
    private float endTime;
    private bool fail;

    private bool endSpawnDelay = false;
    private float endSpawnDelayTime;

    //Set to true by SpawnManager.cs when it finishes spawning enemies
    public bool finishSpawning = false;

    // Start is called before the first frame update
    void Awake()
    {
        enemyUpdate = Random.Range(3, 6);
        cloudUpdate = Random.Range(0, 3);

    }

    // Update is called once per frame
    void Update()
    {
        //Pause the game when the player presses Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.AlternatePause();
        }

        //Prepare to end the game
        if (endGame)
        {
            if(Time.time > endTime)
            {
                Menu.GameOverMenu(endMessage, fail, currentPoints);
                endGame = false;
            }
        }

        //spawn cloud at random time (0 - 2 seconds)
        if (Time.time >= cloudUpdate) {
            int random = Random.Range(0, 3);
            cloudUpdate = Mathf.FloorToInt(Time.time) + random;
            UpdateCloudEverySecond();
        }

        if (finishSpawning)
        {
            //Give the player the opportunity to kill last enemies before points check and boss spawn
            if (!endSpawnDelay)
            {
                endSpawnDelay = true;
                endSpawnDelayTime = Time.time + 5;
                return;
            } else
            {
                if(Time.time < endSpawnDelayTime)
                {
                    return;
                }
            }

            //Player does not have enough points, prepare to end the game
            if(currentPoints < requiredPoints)
            {
                endGame = true;
                endTime = Time.time + 3;
                fail = true;
                endMessage = "Mission Failed: Not Enough Points";
            }
            //Player has enough points, prepare to spawn the boss
            else
            {
                if (bossSpawnTime == 0)
                {
                    bossSpawnTime = Time.time;
                }
                Menu.Warning();
                if (bossSpawnTime + 6 < Time.time)
                {
                    Instantiate(bossPrefab, new Vector3(0, 3, 0), Quaternion.identity);
                    Menu.StopWarning();
                    finishSpawning = false;
                }
            }
        }
    }

    //Instantiate a cloud object at a random area on top of screen
    void UpdateCloudEverySecond()
    {
        Vector3 cloudPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        GameObject go1 = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
    }


    // Description: Function called by EnemyPlane class on enemy death. Increment current points and update scoreboard information
    public void notifyKill(int points, string name)
    {
        //ITS OVER 9000!!!!
        //Player kills the boss, prepare to end the game
        if(points > 9000)
        {
            currentPoints += 500;
            pointsUI.setPointsText(currentPoints, requiredPoints);
            scoreboard.UpdateList(name, 500);
            endGame = true;
            fail = false;
            endMessage = "Mission Complete";
            endTime = Time.time + 5;
            return;
        } else if(points < 0)
        {
            //Player death, prepare to end the game
            endGame = true;
            fail = true;
            endMessage = "Wow, you suck";
            endTime = Time.time + 3;
            return;
        }
        currentPoints += points;
        pointsUI.setPointsText(currentPoints, requiredPoints);
        scoreboard.UpdateList(name, points);
    }
}
