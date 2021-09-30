using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class SpawnManager : MonoBehaviour
{
    //Enemies that can be instantiated during a Scene.
    /**0. BF109E
     * 1. BF109F
     * 2. Do217
     * 3. Me262
     * 4. Me163
     * 5. He111
     * 6. Ju87
     * 7. Ju87G
     * 8. Ju88
    **/

    [System.Serializable]
    public struct EnemyDetails
    {
        public GameObject prefab;
        public int cost;
    }

    //********Player must edit this array in Unity inspector in order to spawn enemies in Level*************
    public EnemyDetails[] EnemyData;


    //Stores information on how to spawn groups of enemies
    [System.Serializable]
    public struct Squadron
    {
        //Array of planes to spawn
        public string planes;
        //Spawn enemies one at a time in rapid succession
        public bool staggered;

        public Squadron(string planes, bool staggered, int cost)
        {
            this.planes = planes;
            this.staggered = staggered;
        }
    }


    // Stores planes that will spawn
    public Squadron[] enemySquadron;

    // Stores indices of planes that will spawn
    private int[] enemyIndices;


    public int spawnIndex = 0;

    private bool staggeredSpawn = false;
    private int staggeredIndex;

    private float enemyUpdate;

    public PointsUI pointsUI;
    public game game;

    // Start is called before the first frame update
    void Awake()
    {
        enemyUpdate = Random.Range(3, 6);
        //Calculate total points for game
        int points = 0;
        for(int i = 0; i < enemySquadron.Length; i++)
        {
            string[] index = enemySquadron[i].planes.Split(',');
            for(int j = 0; j < index.Length; j++)
            {
                points += EnemyData[Int32.Parse(index[j])].cost;
            }
        }

        //TODO update UI and game.cs points
        game.totalPoints = points;
        game.requiredPoints = points * 3 / 4;
        pointsUI.setPointsText(0, points * 3 / 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= enemyUpdate && spawnIndex < enemySquadron.Length)
        {
            

            if (staggeredSpawn)
            {
                if(staggeredIndex < enemyIndices.Length)
                {
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
                    spawnPosition.z = 0;
                    SpawnOneEnemy(enemyIndices[staggeredIndex], spawnPosition);
                    staggeredIndex++;
                    enemyUpdate = Time.time + 1f;
                } else
                {
                    spawnIndex++;
                    //enemy spawn cooldown 2 - 4 seconds
                    enemyUpdate = Time.time + Random.Range(2, 5);
                    staggeredSpawn = false;
                }
            }
            else
            {
                Squadron enemies = enemySquadron[spawnIndex];
                string[] indices = enemies.planes.Split(',');
                enemyIndices = Array.ConvertAll(indices, int.Parse);
                //SPAWN A GROUP OF ENEMIES IN RAPID SUCCESSION
                if (enemies.staggered)
                {
                    staggeredSpawn = true;
                    staggeredIndex = 0;
                } else
                {
                    //SPAWN A GROUP OF ENEMIES OR ONE
                    SpawnSquadron(enemyIndices);
                    spawnIndex++;
                    //enemy spawn cooldown 2 - 4 seconds
                    enemyUpdate = Time.time + Random.Range(2, 5);
                }
            }
        }

        if(spawnIndex == enemySquadron.Length)
        {
            game.finishSpawning = true;
            spawnIndex++;
        }
    }
    //Spawn a single enemy object at a random area on top of screen
    void SpawnOneEnemy(int enemy, Vector3 spawnPosition)
    {
        GameObject go = null;
        go = Instantiate(EnemyData[enemy].prefab, spawnPosition, Quaternion.identity);
        EnemyPlane plane = go.GetComponent<EnemyPlane>();
        FindObjectOfType<DialogueManager>().CreateRandomSpawnEnemyText(go, plane._name, 10);
    }

    /*Spawn a group of enemies at a random area on top of screen in a V formation
     * How it works by example: 
     * int[] planes.Length == 3
     * 
     *      2
     * 1        3
     * 
     * int[] squadron.Length == 6
     *           3
     *      2        4
     * 1                  5
     *                         6
     */
    void SpawnSquadron(int[] squadron)
    {
        Vector3 centerPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(200, Screen.width - 200), Screen.height, Camera.main.farClipPlane / 2));
        centerPosition.z = 0;

        float xPositions = 0;
        float yPositions = 0;
        int middlePlane = squadron.Length / 2;

        //Spawn enemies in a V formation
        for (int i = 0; i < squadron.Length; i++)
        {
            if (i == middlePlane)
            {
                xPositions = centerPosition.x;
                yPositions = centerPosition.y;
            }
            //left
            if (i < middlePlane)
            {
                xPositions = centerPosition.x - (middlePlane - i) * .75f;
                yPositions = centerPosition.y + (middlePlane - i) * .75f;
            }
            //right
            if (i > middlePlane)
            {
                xPositions = centerPosition.x + (i - middlePlane) * .75f;
                yPositions = centerPosition.y + (i - middlePlane) * .75f;
            }
            SpawnOneEnemy(squadron[i], new Vector3(xPositions, yPositions, 0));
        }

    }
}
