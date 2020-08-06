using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject GunTree;
    public GameObject MissileTree;
    public GameObject EscortTree;
    public GameObject Info;

    public Sprite[] sprites;

    /*
     * 0. Triple Shot 
     * 1. Quad Shot
     * 2. Upgrade Triple Shot
     * 3. Double Missile
     * 4. Triple Missile
     * 5. Swarmer Missile
     * 6. Double Escort
     * 7. Triple Escort
     * 8. Quad Escort
    */
    private bool[] toggles = new bool[9];

    private int powerupselect;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
        InitializeGunUpgrades(data.shootType);
        InitializeMissileUpgrades(data.missileType);
        InitializeEscortUpgrades(data.escortType - 1);
    }

    void InitializeGunUpgrades(int shootType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < shootType + 1; i++)
        {
            GameObject toggleObject = GunTree.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (shootType < 3)
            GunTree.transform.GetChild(shootType + 1).gameObject.GetComponent<Toggle>().interactable = true;
    }

    void InitializeMissileUpgrades(int missileType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < missileType + 1; i++)
        {
            GameObject toggleObject = MissileTree.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (missileType < 3)
            MissileTree.transform.GetChild(missileType + 1).gameObject.GetComponent<Toggle>().interactable = true;
    }

    void InitializeEscortUpgrades(int escortType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < escortType + 1; i++)
        {
            GameObject toggleObject = EscortTree.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (escortType < 3)
            EscortTree.transform.GetChild(escortType + 1).gameObject.GetComponent<Toggle>().interactable = true;
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    //TODO check if player has enough money(pts) to get upgrade
    public void Upgrade()
    {
        Info.SetActive(false);
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));

        //Figure out which powerup was selected, modify the save data, let the next upgrade be available
        if(powerupselect > 0)
        {
            if(powerupselect > 9000)
            {
                data.escortType = powerupselect - 9000;
                InitializeEscortUpgrades(data.escortType - 1);
            } else
            {
                data.shootType = powerupselect;
                InitializeGunUpgrades(data.shootType);
            }
        } else
        {
            data.missileType = Mathf.Abs(powerupselect);
            InitializeMissileUpgrades(data.missileType);
        }
        SaveSystem.SavePlayerData(data);
    }

    public void Cancel()
    {
        Info.SetActive(false);
    }

    //Set all booleans in toggles to false except index
    void SetAllSelectToFalse(int index)
    {
        for(int i = 0; i < toggles.Length; i++)
        {
            if(i != index)
            {
                toggles[i] = false;
            }
        }
    }

    public void TripleShotSelect()
    {
        toggles[0] = toggles[0] ? false : true;
        SetAllSelectToFalse(0);
        if (toggles[0])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[0];
            powerupselect = 1;
        } else
        {
            Info.SetActive(false);
        }
    }

    public void QuadShotSelect()
    {
        toggles[1] = toggles[1] ? false : true;
        SetAllSelectToFalse(1);
        if (toggles[1])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quad Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 4 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[1];
            powerupselect = 2;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void UpgradeTripleShotSelect()
    {
        toggles[2] = toggles[2] ? false : true;
        SetAllSelectToFalse(2);
        if (toggles[2])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Upgrade Triple Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 bullets at a time. Center bullet does increased damage.";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[2];
            powerupselect = 3;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void DoubleMissileSelect()
    {
        toggles[3] = toggles[3] ? false : true;
        SetAllSelectToFalse(3);
        if (toggles[3])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 2 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[3];
            powerupselect = -1;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void TripleMissileSelect()
    {
        toggles[4] = toggles[4] ? false : true;
        SetAllSelectToFalse(4);
        if (toggles[4])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[4];
            powerupselect = -2;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void SwarmerMissileSelect()
    {
        toggles[5] = toggles[5] ? false : true;
        SetAllSelectToFalse(5);
        if (toggles[5])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Swarmer Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 8 swarmer missiles at a time. Swarmer missiles do half damage of regular missiles";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[5];
            powerupselect = -3;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void DoubleEscortSelect()
    {
        toggles[6] = toggles[6] ? false : true;
        SetAllSelectToFalse(6);
        if (toggles[6])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "2 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[6];
            powerupselect = 9002;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void TripleEscortSelect()
    {
        toggles[7] = toggles[7] ? false : true;
        SetAllSelectToFalse(7);
        if (toggles[7])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "3 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[7];
            powerupselect = 9003;
        }
        else
        {
            Info.SetActive(false);
        }
    }

    public void QuadEscortSelect()
    {
        toggles[8] = toggles[8] ? false : true;
        SetAllSelectToFalse(8);
        if (toggles[8])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quad Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "4 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[8];
            powerupselect = 9004;
        }
        else
        {
            Info.SetActive(false);
        }
    }
}
