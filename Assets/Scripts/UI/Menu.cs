using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

public class Menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject GameOverMenuUI;
    public TextMeshProUGUI GameOverText;
    public Animation damageAnimator;
    public Animation spawnBossAnimator;
    public GameObject WarningMenuUI;
    public Scoreboard scoreboard;

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        FindObjectOfType<AudioManager>().Play("Pause");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void GameOverMenu(string text)
    {
        GameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        GameOverText.text = text;
        scoreboard.DisplayScoreboard();
    }

    public void Retry()
    {
        GameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Upgrade");
    }

    public void TakeDamage()
    {
        damageAnimator.Play("TakeDamageAnimation");
    }

    public void Warning()
    {
        if (!WarningMenuUI.activeSelf)
        {
            WarningMenuUI.SetActive(true);
            spawnBossAnimator.Play("WarningAnimation");
        }
    }

    public void StopWarning()
    {
        WarningMenuUI.SetActive(false);
        spawnBossAnimator.Stop("WarningAnimation");
    }
}
