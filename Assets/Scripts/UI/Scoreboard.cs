using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public GameObject ScoreObjectPrefab;
    private bool displayed = false;

    public struct Enemies
    {
        public string name;
        public int score;
        public int count;

        public Enemies(string name, int score, int count)
        {
            this.name = name;
            this.score = score;
            this.count = count;
        }
    }

    public List<Enemies> EnemiesList = new List<Enemies>();

    public void UpdateList(string name, int score)
    {
        int index = EnemiesList.FindIndex(enemy => enemy.name == name);
        if(index == -1)
        {
            EnemiesList.Add(new Enemies(name, score, 1));
        } else
        {
            EnemiesList[index] = new Enemies(name, EnemiesList[index].score + score, EnemiesList[index].count + 1);
        }
    }

    public void DisplayScoreboard()
    {
        if (displayed)
            return;

        float scoreObjectHeight = gameObject.GetComponent<RectTransform>().sizeDelta.y / EnemiesList.Count;
        //Default height of UI object
        if(scoreObjectHeight > 25)
        {
            scoreObjectHeight = 25;
        }

        float heightOffset = 0;
        for(int i = 0; i < EnemiesList.Count; i++)
        {
            GameObject go = Instantiate(ScoreObjectPrefab, GameObject.Find("Scoreboard").transform, false);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, scoreObjectHeight);
            Vector3 pos = go.transform.localPosition;
            pos.y -= heightOffset;
            go.transform.localPosition = pos;

            heightOffset += scoreObjectHeight;

            go.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = EnemiesList[i].name;
            go.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x" + EnemiesList[i].count.ToString();
            go.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = EnemiesList[i].score.ToString();
        }
        //Display Total points
        GameObject go1 = Instantiate(ScoreObjectPrefab, GameObject.Find("Scoreboard").transform, false);
        RectTransform rt1 = go1.GetComponent<RectTransform>();
        rt1.sizeDelta = new Vector2(rt1.sizeDelta.x, scoreObjectHeight);
        Vector3 pos1 = go1.transform.localPosition;
        pos1.y -= heightOffset;
        go1.transform.localPosition = pos1;


        go1.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Total: ";
        int total = 0;
        foreach(Enemies enemy in EnemiesList)
        {
            total += enemy.score;
        }
        go1.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = total.ToString();

        displayed = true;
    }
}
