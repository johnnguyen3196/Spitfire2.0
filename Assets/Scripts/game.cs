using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class game : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab4;
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
        enemy = Random.Range(0, 2);
        Debug.Log(enemy);
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Screen.height, Camera.main.farClipPlane / 2));
        switch (enemy)
        {
            case 0:
                GameObject go1 = Instantiate(enemyPrefab1, screenPosition, Quaternion.identity);
                break;
            case 1:
                GameObject go4 = Instantiate(enemyPrefab4, screenPosition, Quaternion.identity);
                break;
        }
    }
}
