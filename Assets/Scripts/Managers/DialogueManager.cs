using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    public GameObject textBubblePrefab;

    //2D string array
    public MultiDimentionalString[] spawnText;

    public string[] deathText;

    public DialogueManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Create(GameObject target, string text)
    {
        GameObject go = Instantiate(textBubblePrefab, target.transform.position, Quaternion.identity);
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        TextBubble textBubble = go.GetComponent<TextBubble>();
        textBubble.target = target.gameObject;
        textBubble.UpdateText(text);
    }

    public void CreateRandomSpawnEnemyText(GameObject target, int enemy)
    {
        if (enemy >= spawnText.Length)
            return;

        //Enemy will spawn with dialogue 10% of the time
        if(Random.Range(0, 10) == 0)
            Create(target, spawnText[enemy].strings[Random.Range(0, spawnText[enemy].strings.Length)]);
    }

    public void CreateEnemyDeathText(GameObject target)
    {
        //Enemy will have death message show 20% of the time
        if (Random.Range(0, 5) == 0)
            Create(target, deathText[Random.Range(0, deathText.Length)]);
    }
}
