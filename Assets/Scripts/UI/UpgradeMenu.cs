using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject GunTreeObject;
    public GameObject MissileTreeObject;
    public GameObject EscortTreeObject;
    public GameObject Info;
    public GameObject PointsInfo;
    public GameObject Button;
    public GameObject EquippedObject;

    public Sprite[] gunSprites;
    public Sprite[] missileSprites;
    public Sprite[] escortSprites;

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

    private int points;

    [System.Serializable]
    public struct Node {
        public int powerup;
        public int tier;
        public int ObjectPos;
        public int[] Children;
    }

    public Node[] GunTree;
    public Node[] MissileTree;
    public Node[] EscortTree;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
        InitializeGunUpgrades(data.shootType);
        InitializeMissileUpgrades(data.missileType);
        InitializeEscortUpgrades(data.escortType - 1);
        points = data.points;
        SetPointsText(points);
        SetEquippedImage(data);
    }

    void SetPointsText(int points)
    {
        PointsInfo.GetComponent<TextMeshProUGUI>().text = "Pts: " + points.ToString();
    }

    void SetEquippedImage(PlayerData data)
    {
        EquippedObject.transform.GetChild(0).GetComponent<Image>().sprite = gunSprites[data.shootType];
        EquippedObject.transform.GetChild(1).GetComponent<Image>().sprite = missileSprites[data.missileType];
        EquippedObject.transform.GetChild(2).GetComponent<Image>().sprite = escortSprites[data.escortType - 1];
    }

    void InitializeGunUpgrades(int shootType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < shootType + 1; i++)
        {
            GameObject toggleObject = GunTreeObject.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (shootType < 3)
            GunTreeObject.transform.GetChild(shootType + 1).gameObject.GetComponent<Toggle>().interactable = true;
    }

    void InitializeMissileUpgrades(int missileType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < missileType + 1; i++)
        {
            GameObject toggleObject = MissileTreeObject.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (missileType < 3)
            MissileTreeObject.transform.GetChild(missileType + 1).gameObject.GetComponent<Toggle>().interactable = true;
    }

    void InitializeEscortUpgrades(int escortType)
    {
        //disable toggle for upgrades player already has
        for (int i = 0; i < escortType + 1; i++)
        {
            GameObject toggleObject = EscortTreeObject.transform.GetChild(i).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.enabled = false;

            Image image = toggleObject.GetComponentInChildren<Image>();
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        //enable toggle for next upgrade
        if (escortType < 3)
            EscortTreeObject.transform.GetChild(escortType + 1).gameObject.GetComponent<Toggle>().interactable = true;
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

    void DisableUpgradeButton(int cost)
    {
        if(points >= cost)
        {
            Button.GetComponent<Button>().interactable = true;
        } else
        {
            Button.GetComponent<Button>().interactable = false;
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[1];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupselect = 1;
        } else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1000);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[2];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupselect = 2;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1500);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[3];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = 3;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[1];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupselect = -1;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1000);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[2];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupselect = -2;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1500);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[3];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = -3;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = escortSprites[1];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupselect = 9002;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1000);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = escortSprites[2];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupselect = 9003;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1500);
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
            Info.transform.GetChild(3).GetComponent<Image>().sprite = escortSprites[3];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = 9004;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
    }
}
