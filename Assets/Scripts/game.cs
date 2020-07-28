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
    public GameObject bossPrefab;

    public PointsUI pointsUI;
    public Menu Menu;

    private float enemyUpdate;
    private int cloudUpdate;

    private int[] enemyTypes;
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
    private int[] enemyPoints = new int[] { 10, 20, 50, 30, 30, 50, 20, 30, 40 };

    //Stores information on how to spawn groups of enemies
    private struct Squadron
    {
        public int[] planes;
        public bool staggered;

        public Squadron(int[] planes, bool staggered)
        {
            this.planes = planes;
            this.staggered = staggered;
        }
    }

    private Squadron[] enemySquadron =
        new Squadron[] {
            new Squadron{planes = new int[] {0, 0, 0}, staggered = false},
            new Squadron{planes = new int[] {0, 0, 0, 0}, staggered = false},
            new Squadron{planes = new int[] {0, 1, 0}, staggered = false},
            new Squadron{planes = new int[] { 3, 3, 3, 3, 3 }, staggered = true},
            new Squadron{planes = new int[] {3, 3}, staggered = false},
            new Squadron{planes = new int[] {4, 4}, staggered = false},
            new Squadron{planes = new int[] {4, 4, 4, 4}, staggered = true},
            new Squadron{planes = new int[] { 0, 0, 5, 0, 0 }, staggered = false},
            new Squadron{planes = new int[] { 1, 5, 1 }, staggered = false},
            new Squadron{planes = new int[] {0, 0, 0, 5, 5, 0, 0, 0,}, staggered = false}
        };

    //Helps spawn different types of enemies and squadrons for the game
    private struct SpawnType
    {
        //do we need to spawn a squadron or just 1 enemy?
        public bool squadron;
        //what type of squadron do we spawn?
        public int squadronIndex;
        //what type of SINGLE enemy do we spawn?
        public int enemyType;

        public SpawnType(bool squadron, int squadronIndex, int enemyType)
        {
            this.squadron = squadron;
            this.squadronIndex = squadronIndex;
            this.enemyType = enemyType;
        }
    }

    private List<SpawnType> EnemiesSpawn = new List<SpawnType>();

    private int spawnIndex = 0;

    private bool staggeredSpawn;
    private int staggeredIndex;

    private bool spawnBoss = false;
    //edge case. Only spawn the boss once
    private bool bossSpawned = false;

    public int totalPoints;
    public int currentPoints = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyUpdate = Random.Range(3, 6);
        cloudUpdate = Random.Range(0, 3);

        totalPoints = 3000;
        //create a list of enemy spawns that add up to atleast 3000 pts
        while(totalPoints > 0)
        {
            int spawnSquadron = Random.Range(0, 2);
            //spawn only 1 enemy
            if(spawnSquadron == 0)
            {
                int enemy = Random.Range(0, enemyPoints.Length);
                EnemiesSpawn.Add(new SpawnType(false, 0, enemy));
                totalPoints -= enemyPoints[enemy];
            } else
            {
                //spawn a squadron of enemies
                int squadron = Random.Range(0, enemySquadron.Length);
                EnemiesSpawn.Add(new SpawnType(true, squadron, 0));
                for(int i = 0; i < enemySquadron[squadron].planes.Length; i++)
                {
                    totalPoints -= enemyPoints[enemySquadron[squadron].planes[i]];
                }
            }
        }
        //Player only needs to score only 80% of the enemies spawned
        totalPoints = 2000;
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
            if (staggeredSpawn)
            {
                if (staggeredIndex < enemySquadron[EnemiesSpawn[spawnIndex].squadronIndex].planes.Length)
                {
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
                    SpawnOneEnemy(enemySquadron[EnemiesSpawn[spawnIndex].squadronIndex].planes[staggeredIndex], spawnPosition);
                    staggeredIndex++;
                    enemyUpdate += Time.time + .5f;
                } else
                {
                    spawnIndex++;
                    //enemy spawn cooldown 3-5 seconds
                    enemyUpdate = Time.time + Random.Range(3, 6);
                    staggeredSpawn = false;
                }
            }
            else
            {
                SpawnType spawn = EnemiesSpawn[spawnIndex];
                if (spawn.squadron)
                {
                    //SPAWN A GROUP OF ENEMIES IN RAPID SUCCESSION
                    if (enemySquadron[spawn.squadronIndex].staggered)
                    {
                        staggeredSpawn = true;
                        staggeredIndex = 0;
                    }
                    else
                    {
                        //SPAWN A GROUP OF ENEMIES
                        SpawnSquadron(spawn.squadronIndex);
                        spawnIndex++;
                        //enemy spawn cooldown 3-5 seconds
                        enemyUpdate = Time.time + Random.Range(3, 6);
                    }
                }
                else
                {
                    //SPAWN ONE ENEMY
                    //spawn in random position on top of screen
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
                    spawnPosition.z = 0;
                    SpawnOneEnemy(spawn.enemyType, spawnPosition);
                    spawnIndex++;
                    //enemy spawn cooldown 2-4 seconds
                    enemyUpdate = Time.time + Random.Range(2, 5);
                }
            }
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

    void SpawnOneEnemy(int enemy, Vector3 spawnPosition)
    {
        switch (enemy)
        {
            case 0:
                GameObject go1 = Instantiate(enemyPrefab1, spawnPosition, Quaternion.identity);
                break;
            case 1:
                GameObject go2 = Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
                break;
            case 2:
                GameObject go3 = Instantiate(enemyPrefab3, spawnPosition, Quaternion.identity);
                break;
            case 3:
                GameObject go4 = Instantiate(enemyPrefab4, spawnPosition, Quaternion.identity);
                break;
            case 4:
                GameObject go5 = Instantiate(enemyPrefab5, spawnPosition, Quaternion.identity);
                break;
            case 5:
                GameObject go6 = Instantiate(enemyPrefab6, spawnPosition, Quaternion.identity);
                break;
            case 6:
                GameObject go7 = Instantiate(enemyPrefab7, spawnPosition, Quaternion.identity);
                break;
            case 7:
                GameObject go8 = Instantiate(enemyPrefab8, spawnPosition, Quaternion.identity);
                break;
            case 8:
                GameObject go9 = Instantiate(ju88Prefab, spawnPosition, Quaternion.identity);
                break;
        }
    }

    void SpawnSquadron(int squadron)
    {
        Vector3 centerPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(50, Screen.width-50), Screen.height, Camera.main.farClipPlane / 2));
        centerPosition.z = 0;

        int[] planes = enemySquadron[squadron].planes;
        float xPositions = 0;
        float yPositions = 0;
        int middlePlane = planes.Length / 2;

        //Spawn enemies in a V formation
        for(int i = 0; i < planes.Length; i++)
        {
            if(i == middlePlane)
            {
                xPositions = centerPosition.x;
                yPositions = centerPosition.y;
            }
            //left
            if(i < middlePlane)
            {
                xPositions = centerPosition.x - (middlePlane - i) * .75f;
                yPositions = centerPosition.y + (middlePlane - i) * .75f;
            }
            //right
            if(i > middlePlane)
            {
                xPositions = centerPosition.x + (i - middlePlane) * .75f;
                yPositions = centerPosition.y + (i - middlePlane) * .75f;
            }
            SpawnOneEnemy(planes[i], new Vector3(xPositions, yPositions, 0));
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
