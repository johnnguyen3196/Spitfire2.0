using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadMenu;

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
}
