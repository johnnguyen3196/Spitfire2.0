using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerCreation : MonoBehaviour
{
    public InputField mainInputField;
    public Button playButton;

    public GameObject[] saveSlots;
    public GameObject saveMenu;
    public GameObject creationMenu;

    private string saveName;
    //0. Spitfire
    //1. Mustang
    private int plane = -1;

    public Sprite spitfire;
    public Sprite mustang;

    List<PlayerData> dataList;

    private int slot;

    void Start()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/");
        dataList = new List<PlayerData>();

        foreach (string s in files)
        {
            if (s.Contains(".save"))
            {
                string[] tokens = s.Split('/');
                dataList.Add(SaveSystem.LoadPlayer(tokens[tokens.Length - 1].Substring(0, (tokens[tokens.Length - 1].Length - 5))));
            }
        }
        //sort by slot number
        dataList.Sort((x, y) => x.slot.CompareTo(y.slot));

        //show previous save information
        foreach (PlayerData data in dataList)
        {
            foreach (Transform t in saveSlots[data.slot].transform.GetChild(0))
            {
                if (t.name == "SaveName")
                {
                    t.GetComponent<TextMeshProUGUI>().text = data.saveName;
                }
                else if (t.name == "Mission")
                {
                    //temp
                    t.GetComponent<TextMeshProUGUI>().text = "Mission: 1";
                    t.gameObject.SetActive(true);
                }
                else if (t.name == "PlaneImage")
                {
                    switch (data.plane)
                    {
                        case 0:
                            t.GetComponent<Image>().sprite = spitfire;
                            break;
                        case 1:
                            t.GetComponent<Image>().sprite = mustang;
                            break;
                    }
                }
            }
        }
    }

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
        player.shootType = 0;
        player.missileType = 0;
        player.escortType = 1;

        PlayerData data = SaveSystem.SavePlayer(player, saveName, slot);

        PlayerPrefs.SetString("saveName", saveName);

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

    public void SaveSlot0()
    {
        foreach (PlayerData data in dataList)
        {
            if(data.slot == 0)
                File.Delete(Application.persistentDataPath + "/" + data.saveName + ".save");
        }
        saveMenu.SetActive(false);
        creationMenu.SetActive(true);
        slot = 0;
    }
    public void SaveSlot1()
    {
        foreach (PlayerData data in dataList)
        {
            if (data.slot == 1)
                File.Delete(Application.persistentDataPath + "/" + data.saveName + ".save");
        }
        saveMenu.SetActive(false);
        creationMenu.SetActive(true);
        slot = 1;
    }
    public void SaveSlot2()
    {
        foreach (PlayerData data in dataList)
        {
            if (data.slot == 2)
                File.Delete(Application.persistentDataPath + "/" + data.saveName + ".save");
        }
        saveMenu.SetActive(false);
        creationMenu.SetActive(true);
        slot = 2;
    }
}
