using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public Sprite spitfire;
    public Sprite mustang;

    public GameObject[] loadSlots;

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
            foreach (Transform t in loadSlots[data.slot].transform.GetChild(0))
            {
                if (t.name == "SaveName")
                {
                    t.GetComponent<TextMeshProUGUI>().text = data.saveName;
                }
                else if (t.name == "Mission")
                {
                    //temp
                    t.GetComponent<TextMeshProUGUI>().text = "Mission: " + data.level;
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
                loadSlots[data.slot].GetComponentInChildren<Button>().interactable = true;
            }
        }
    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadScene("Level" + dataList[index].level.ToString());
    }

    public void LoadButton0()
    {
        string saveName = null;
        foreach(PlayerData data in dataList)
        {
            if (data.slot == 0)
            {
                saveName = data.saveName;
            }
        }
        PlayerPrefs.SetString("saveName", saveName);

        LoadLevel(0);
    }

    public void LoadButton1()
    {
        string saveName = null;
        foreach (PlayerData data in dataList)
        {
            if (data.slot == 1)
            {
                saveName = data.saveName;
            }
        }
        PlayerPrefs.SetString("saveName", saveName);

        LoadLevel(1);
    }

    public void LoadButton2()
    {
        string saveName = null;
        foreach (PlayerData data in dataList)
        {
            if (data.slot == 2)
            {
                saveName = data.saveName;
            }
        }
        PlayerPrefs.SetString("saveName", saveName);

        LoadLevel(2);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
