using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    public GameObject textBubblePrefab;

    [System.Serializable]
    public struct EnemyDialogue
    {
        public string Name;
        public string[] Dialogue;
    }

    public EnemyDialogue[] EnemyDialogues;

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

    public void CreateRandomSpawnEnemyText(GameObject target, string enemy, int percentage)
    {
        //Enemy will spawn with dialogue "percentage" of the time
        if (Random.Range(0, 100) < percentage)
        {
            int index = Array.FindIndex(EnemyDialogues, dialogue => dialogue.Name == enemy);
            if(index == -1)
            {
                Debug.LogWarning("Dialogue for " + enemy + " not found!");
                return;
            }

            Create(target, EnemyDialogues[index].Dialogue[Random.Range(0, EnemyDialogues[index].Dialogue.Length)]);
        }
    }

    public void CreateEnemyText(GameObject target, string enemy, int dialogueIndex)
    {
        int index = Array.FindIndex(EnemyDialogues, dialogue => dialogue.Name == enemy);
        if (index == -1)
        {
            Debug.LogWarning("Dialogue for " + enemy + " not found!");
            return;
        }
        if(dialogueIndex > EnemyDialogues[index].Dialogue.Length)
        {
            Debug.LogWarning("Dialogue for " + enemy + " at " + dialogueIndex + " does not exist");
            return;
        }
        Create(target, EnemyDialogues[index].Dialogue[dialogueIndex]);
    }

    public void CreateEnemyDeathText(GameObject target)
    {
        //Enemy will have death message show 20% of the time
        if (Random.Range(0, 5) == 0)
            Create(target, deathText[Random.Range(0, deathText.Length)]);
    }
}
