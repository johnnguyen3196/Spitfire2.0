using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using System.Reflection;
using System.Linq;

public class game : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    
    public GameObject cloudPrefab;

    public GameObject bossPrefab;

    public PointsUI pointsUI;
    public Menu Menu;
    public Scoreboard scoreboard;

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
    public int[] enemyPoints;

    //only these enemies will spawn in this level
    public int[] singleEnemies;

    //Stores information on how to spawn groups of enemies
    [System.Serializable]
    public struct Squadron
    {
        public int[] planes;
        public bool staggered;

        public Squadron(int[] planes, bool staggered)
        {
            this.planes = planes;
            this.staggered = staggered;
        }
    }

    public Squadron[] enemySquadron;
        

    //Helps spawn different types of enemies and squadrons for the game
    private struct SpawnType
    {
        //do we need to spawn a squadron or just 1 enemy?
        public bool squadron;
        //what type of squadron do we spawn?
        public int squadronIndex;
        //what type of SINGLE enemy do we spawn?
        public int enemyType;
        public int cost;

        public SpawnType(bool squadron, int squadronIndex, int enemyType, int cost)
        {
            this.squadron = squadron;
            this.squadronIndex = squadronIndex;
            this.enemyType = enemyType;
            this.cost = cost;
        }
    }

    private List<SpawnType> EnemiesSpawn = new List<SpawnType>();

    public int spawnIndex = 0;

    private bool staggeredSpawn;
    private int staggeredIndex;

    private float bossSpawnTime = 0;

    private bool endGame = false;
    private string endMessage;
    private float endTime;
    private bool fail;

    public int totalPoints;
    public int requiredPoints;
    public int currentPoints = 0;

    public int level;

    // Start is called before the first frame update
    void Start()
    {
        //for some stupid reason, when the player retries a mission, the timescale is set to 0. idk why
        Time.timeScale = 1;

        enemyUpdate = Random.Range(3, 6);
        cloudUpdate = Random.Range(0, 3);

        //create a list of enemy spawns that add up to atleast 3000 pts
        while(totalPoints > 0)
        {
            //spawn 1 enemy 70% of the time, squadron 30%
            int spawnSquadron = Random.Range(0, 10);
            //spawn only 1 enemy
            if(spawnSquadron < 7)
            {
                int enemy = singleEnemies[Random.Range(0, singleEnemies.Length)];
                if(enemyPoints[enemy] > totalPoints)
                {
                    break;
                }
                EnemiesSpawn.Add(new SpawnType(false, 0, enemy, enemyPoints[enemy]));
                totalPoints -= enemyPoints[enemy];
            } else
            {
                //spawn a squadron of enemies
                int squadron = Random.Range(0, enemySquadron.Length);
                if (TotalCostOfSquadron(enemySquadron[squadron].planes) > totalPoints)
                {
                    break;
                }
                EnemiesSpawn.Add(new SpawnType(true, squadron, 0, TotalCostOfSquadron(enemySquadron[squadron].planes)));
                totalPoints -= TotalCostOfSquadron(enemySquadron[squadron].planes);
            }
        }
        if(totalPoints > 0)
        {
            fillRemainingPoints();
        }
        pointsUI.setPointsText(0, requiredPoints);
    }

    private void fillRemainingPoints()
    {
        if (totalPoints <= 0)
            return;
        int largestSquadronIndex = -1;
        for(int i = 0; i < enemySquadron.Length; i++)
        {
            if(TotalCostOfSquadron(enemySquadron[i].planes) <= totalPoints)
            {
                if (largestSquadronIndex != -1)
                {
                    if (TotalCostOfSquadron(enemySquadron[i].planes) > TotalCostOfSquadron(enemySquadron[largestSquadronIndex].planes))
                        largestSquadronIndex = i;
                } else
                {
                    largestSquadronIndex = i;
                }
            }
        }

        if (largestSquadronIndex != -1)
        {
            EnemiesSpawn.Add(new SpawnType(true, largestSquadronIndex, 0, TotalCostOfSquadron(enemySquadron[largestSquadronIndex].planes)));
            totalPoints -= TotalCostOfSquadron(enemySquadron[largestSquadronIndex].planes);
        } else
        {
            int largestPlaneIndex = -1;
            for (int i = 0; i < enemyPoints.Length; i++)
            {
                if (enemyPoints[i] <= totalPoints)
                {
                    if (largestPlaneIndex != -1)
                    {
                        if (enemyPoints[i] > enemyPoints[largestPlaneIndex])
                            largestPlaneIndex = i;
                    }
                    else
                    {
                        largestPlaneIndex = i;
                    }
                }
            }
            if (largestPlaneIndex != -1)
            {
                EnemiesSpawn.Add(new SpawnType(false, 0, largestPlaneIndex, enemyPoints[largestPlaneIndex]));
                totalPoints -= enemyPoints[largestPlaneIndex];
            } else
            {
                //This shouldn't happen. But if it does, we will be in an infinite loop
                totalPoints = 0;
            }
        }
        fillRemainingPoints();
    }

    private int TotalCostOfSquadron(int[] planes)
    {
        int totalCost = 0;
        foreach(int plane in planes)
        {
            totalCost += enemyPoints[plane];
        }
        return totalCost;
    }

    // Update is called once per frame
    void Update()
    {
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

        if (spawnIndex == EnemiesSpawn.Count)
        {
            if(currentPoints < requiredPoints)
            {
                endGame = true;
                endTime = Time.time + 3;
                fail = true;
                endMessage = "Mission Failed: Not Enough Points";
                spawnIndex++;
            } else
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
                    spawnIndex++;
                }
            }
        }

        if (Time.time >= enemyUpdate && spawnIndex < EnemiesSpawn.Count)
        {
            if (staggeredSpawn)
            {
                if (staggeredIndex < enemySquadron[EnemiesSpawn[spawnIndex].squadronIndex].planes.Length)
                {
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
                    spawnPosition.z = 0;
                    SpawnOneEnemy(enemySquadron[EnemiesSpawn[spawnIndex].squadronIndex].planes[staggeredIndex], spawnPosition);
                    staggeredIndex++;
                    enemyUpdate = Time.time + 1f;
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
    }

    void UpdateCloudEverySecond()
    {
        Vector3 cloudPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        GameObject go1 = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
    }

    void SpawnOneEnemy(int enemy, Vector3 spawnPosition)
    {
        GameObject go = null;
        go = Instantiate(enemyPrefabs[enemy], spawnPosition, Quaternion.identity);
        FindObjectOfType<DialogueManager>().CreateRandomSpawnEnemyText(go, enemy);
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

    public void notifyKill(int points, string name)
    {
        //ITS OVER 9000!!!!
        if(points > 9000)
        {
            scoreboard.UpdateList(name, 500);
            endGame = true;
            fail = false;
            endMessage = "Mission Complete";
            endTime = Time.time + 5;
            return;
        } else if(points < 0)
        {
            //Player death
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
