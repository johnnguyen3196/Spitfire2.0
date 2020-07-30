using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerCreation : MonoBehaviour
{
    public InputField mainInputField;
    public Button playButton;

    private string saveName;
    //0. Spitfire
    //1. Mustang
    private int plane = -1;

    public void Cancel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Play()
    {
        Player player = new Player();
        player.saveName = saveName;
        player.plane = plane;
        player.level = 1;

        SaveSystem.SavePlayer(player);

        SceneManager.LoadScene("Level1");
    }

    public void PickSpitfire()
    {
        plane = 0;
        enablePlay(mainInputField.text);
    }

    public void PickMustang()
    {
        plane = 1;
        enablePlay(mainInputField.text);
    }

    public void TextChange()
    {
        saveName = mainInputField.text;
        enablePlay(saveName);
    }

    private void enablePlay(string text)
    {
        if(text.Length > 0 && plane != -1)
        {
            playButton.interactable = true;
        }
    }
}
