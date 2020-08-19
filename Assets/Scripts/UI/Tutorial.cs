using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject keyText;
    public GameObject[] highlights;
    public int highlightIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        highlights[highlightIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && highlightIndex != 0 && highlightIndex != highlights.Length - 1)
        {
            SetNextHighlight();
        }
    }

    public void SetNextHighlight()
    {
        Debug.Log(highlightIndex);
        if(highlightIndex != 0 || highlightIndex != highlights.Length - 1)
        {
            keyText.SetActive(true);
            highlights[highlightIndex].SetActive(false);
            highlightIndex++;
            highlights[highlightIndex].SetActive(true);
        } else
        {
            keyText.SetActive(false);
        }
    }

    public void Continue()
    {
        highlights[highlightIndex].SetActive(false);
        keyText.SetActive(false);
        Time.timeScale = 1;
    }

    public void Again()
    {
        highlights[highlightIndex].SetActive(false);
        highlightIndex = 1;
        highlights[highlightIndex].SetActive(true);
    }

    public void No()
    {
        SetNextHighlight();
    }
}
