using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadMenu;
    public GameObject LevelSelectMenu;

    public GameObject[] LevelSelectButtons;

    private PlayerData data = null;
    void Start()
    {
        if(PlayerPrefs.GetString("saveName", "") == "")
        {
            GameObject.Find("LevelSelectButton").GetComponent<Button>().interactable = false;
        } else
        {
            data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
        }
        DisableLevelSelectButtons(data.level);
    }
    
    private void DisableLevelSelectButtons(int level)
    {
        for(int i = level; i < LevelSelectButtons.Length; i++)
        {
            LevelSelectButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerCreation");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Continue()
    {
        LoadMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void LevelSelect() 
    {
        LevelSelectMenu.SetActive(true);
        gameObject.SetActive(false);

    }
    
    public void Back()
    {
        LevelSelectMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }
}
