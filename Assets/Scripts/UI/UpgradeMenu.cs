using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

// Manages all UI elements in the Upgrade Scene
public class UpgradeMenu : MonoBehaviour
{
    public GameObject GunTreeObject;
    public GameObject MissileTreeObject;
    public GameObject EscortTreeObject;
    public GameObject Info;
    public GameObject PointsInfo;
    public GameObject UpgradeButton;
    public GameObject EquipButton;
    public GameObject EquippedObject;

    public Sprite[] gunSprites;
    public Sprite[] missileSprites;
    public Sprite[] escortSprites;

    /*
     * 0. Double Shot
     * 1. Triple Shot 
     * 2. Quad Shot
     * 3. Upgrade Triple Shot
     * 4. Single Missile
     * 5. Double Missile
     * 6. Triple Missile
     * 7. Swarmer Missile
     * 8. Single Escort
     * 9. Double Escort
     * 10. Triple Escort
     * 11. Quad Escort
     * 12. AutoCannon
     * 13. High Velocity Shot
    */
    private bool[] toggles = new bool[50];

    private int powerupselect;

    private int upgradeCost;

    [System.Serializable]
    public struct Node {
        public string name;
        public int powerup;
        public int ObjectPos;
        public int[] Children;
    }

    public Node[] GunTree;
    public Node[] MissileTree;
    public Node[] EscortTree;

    private PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        // load the player's save and initialize the upgrades
        data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
     
        InitializeGunUpgrades(data.researchedGunUpgrades);
        InitializeMissileUpgrades(data.researchedMissileUpgrades);
        InitializeEscortUpgrades(data.researchedEscortUpgrades);

        SetPointsText(data.points);
        SetEquippedImage(data);
    }

    #region Set Texts and Images
    void SetPointsText(int points)
    {
        PointsInfo.GetComponent<TextMeshProUGUI>().text = "Pts: " + points.ToString();
    }

    void SetEquippedImage(PlayerData data)
    {
        EquippedObject.transform.GetChild(0).GetComponent<Image>().sprite = gunSprites[data.shootType];
        EquippedObject.transform.GetChild(1).GetComponent<Image>().sprite = missileSprites[data.missileType - 1];
        EquippedObject.transform.GetChild(2).GetComponent<Image>().sprite = escortSprites[data.escortType - 1];
    }
    #endregion

    #region Initialize all tree upgrades
    void InitializeGunUpgrades(List<int> upgrades)
    {
        foreach(int upgrade in upgrades)
        {
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = GunTreeObject.transform.GetChild(GunTree[upgrade].ObjectPos).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach(int child in GunTree[upgrade].Children)
            {
                GameObject childToggleObject = GunTreeObject.transform.GetChild(GunTree[child].ObjectPos).gameObject;
                Toggle childToggle = childToggleObject.GetComponent<Toggle>();
                childToggle.interactable = true;
            }

            //Set color of researched upgrade image
            ColorBlock cb = toggle.colors;
            Color color = cb.normalColor;
            color.a = 1;
            cb.normalColor = color;
            toggle.colors = cb;
        }
    }

    void InitializeMissileUpgrades(List<int> upgrades)
    {
        foreach (int upgrade in upgrades)
        {
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = MissileTreeObject.transform.GetChild(MissileTree[upgrade].ObjectPos).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach (int child in MissileTree[upgrade].Children)
            {
                GameObject childToggleObject = MissileTreeObject.transform.GetChild(MissileTree[child].ObjectPos).gameObject;
                Toggle childToggle = childToggleObject.GetComponent<Toggle>();
                childToggle.interactable = true;
            }

            //Set color of researched upgrade image
            ColorBlock cb = toggle.colors;
            Color color = cb.normalColor;
            color.a = 1;
            cb.normalColor = color;
            toggle.colors = cb;
        }
    }

    void InitializeEscortUpgrades(List<int> upgrades)
    {
        foreach (int upgrade in upgrades)
        {
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = EscortTreeObject.transform.GetChild(EscortTree[upgrade].ObjectPos).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach (int child in EscortTree[upgrade].Children)
            {
                GameObject childToggleObject = EscortTreeObject.transform.GetChild(EscortTree[child].ObjectPos).gameObject;
                Toggle childToggle = childToggleObject.GetComponent<Toggle>();
                childToggle.interactable = true;
            }

            //Set color of researched upgrade image
            ColorBlock cb = toggle.colors;
            Color color = cb.normalColor;
            color.a = 1;
            cb.normalColor = color;
            toggle.colors = cb;
        }
    }
    #endregion

    public void Play()
    {
        SaveSystem.SavePlayerData(data);
        SceneManager.LoadScene("Level" + data.level);
    }

    public void Quit()
    {
        SaveSystem.SavePlayerData(data);
        SceneManager.LoadScene("Menu");
    }

    #region Info buttons
    public void Upgrade()
    {
        Info.SetActive(false);

        //Figure out which powerup was selected, modify the save data, let the next upgrade be available
        if(powerupselect >= 0)
        {
            if(powerupselect > 9000)
            {
                data.escortType = powerupselect - 9000;
                data.researchedEscortUpgrades.Add(Array.FindIndex(EscortTree, escortUpgrade => escortUpgrade.powerup == powerupselect));
                InitializeEscortUpgrades(data.researchedEscortUpgrades);
            } else
            {
                data.shootType = powerupselect;
                data.researchedGunUpgrades.Add(Array.FindIndex(GunTree, gunUpgrade => gunUpgrade.powerup == powerupselect));
                InitializeGunUpgrades(data.researchedGunUpgrades);
            }
        } else
        {
            data.missileType = Mathf.Abs(powerupselect);
            data.researchedMissileUpgrades.Add(Array.FindIndex(MissileTree, missileUpgrade => missileUpgrade.powerup == powerupselect));
            InitializeMissileUpgrades(data.researchedMissileUpgrades);
        }
        data.points -= upgradeCost;
        SetPointsText(data.points);
        SetEquippedImage(data);
    }

    public void Equip()
    {
        Info.SetActive(false);

        //Figure out which powerup was selected, modify the save data, let the next upgrade be available
        if (powerupselect >= 0)
        {
            if (powerupselect > 9000)
            {
                data.escortType = powerupselect - 9000;
            }
            else
            {
                data.shootType = powerupselect;
            }
        }
        else
        {
            data.missileType = Mathf.Abs(powerupselect);
        }
        SetEquippedImage(data);
    }

    public void Cancel()
    {
        Info.SetActive(false);
    }
    #endregion

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
        
        /*  Upgraded    Can Afford   |   Enable Button
         * --------------------------|----------------
         *      T           T        |         F
         *      T           F        |         F
         *      F           T        |         T
         *      F           F        |         F
        */
        if(data.points >= cost && !CheckResearchedUpgrade(data))
        {
            UpgradeButton.GetComponent<Button>().interactable = true;
        } else
        {
            UpgradeButton.GetComponent<Button>().interactable = false;
        }
    }

    void DisableEquippedButton(PlayerData data)
    {
        //check if player researched this upgrade
        if(CheckResearchedUpgrade(data))
        {
            EquipButton.GetComponent<Button>().interactable = true;
        } else
        {
            EquipButton.GetComponent<Button>().interactable = false;
        }
    }

    bool CheckResearchedUpgrade(PlayerData data)
    {
        if(Array.FindIndex(GunTree, gunUpgrade => gunUpgrade.powerup == powerupselect) != -1)
        {
            if(data.researchedGunUpgrades.FindIndex(upgrade => upgrade == (Array.FindIndex(GunTree, gunUpgrade => gunUpgrade.powerup == powerupselect))) != -1)
            {
                return true;
            }
        }
        if (Array.FindIndex(MissileTree, missileUpgrade => missileUpgrade.powerup == powerupselect) != -1)
        {
            if (data.researchedMissileUpgrades.FindIndex(upgrade => upgrade == (Array.FindIndex(MissileTree, missileUpgrade => missileUpgrade.powerup == powerupselect))) != -1)
            {
                return true;
            }
        }
        if (Array.FindIndex(EscortTree, escortUpgrade => escortUpgrade.powerup == powerupselect) != -1)
        {
            if (data.researchedEscortUpgrades.FindIndex(upgrade => upgrade == (Array.FindIndex(EscortTree, escortUpgrade => escortUpgrade.powerup == powerupselect))) != -1)
            {
                return true;
            }
        }
        return false;
    }

    #region Tree buttons
    public void ShowGunTree()
    {
        GunTreeObject.SetActive(true);
        MissileTreeObject.SetActive(false);
        EscortTreeObject.SetActive(false);
    }

    public void ShowMissileTree()
    {
        GunTreeObject.SetActive(false);
        MissileTreeObject.SetActive(true);
        EscortTreeObject.SetActive(false);
    }

    public void ShowEscortTree()
    {
        GunTreeObject.SetActive(false);
        MissileTreeObject.SetActive(false);
        EscortTreeObject.SetActive(true);
    }

    public void DoubleShotSelect()
    {
        toggles[0] = toggles[0] ? false : true;
        SetAllSelectToFalse(0);
        if (toggles[0])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 2 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[0];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupselect = 0;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void TripleShotSelect()
    {
        toggles[1] = toggles[1] ? false : true;
        SetAllSelectToFalse(1);
        if (toggles[1])
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
        upgradeCost = 1000;
        DisableEquippedButton(data);
    }

    public void QuadShotSelect()
    {
        toggles[2] = toggles[2] ? false : true;
        SetAllSelectToFalse(2);
        if (toggles[2])
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
        upgradeCost = 1500;
        DisableEquippedButton(data);
    }

    public void UpgradeTripleShotSelect()
    {
        toggles[3] = toggles[3] ? false : true;
        SetAllSelectToFalse(3);
        if (toggles[3])
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
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void SingleMissileSelect()
    {
        toggles[4] = toggles[4] ? false : true;
        SetAllSelectToFalse(4);
        if (toggles[4])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Single Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 1 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[0];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupselect = -1;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void DoubleMissileSelect()
    {
        toggles[5] = toggles[5] ? false : true;
        SetAllSelectToFalse(5);
        if (toggles[5])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 2 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[1];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupselect = -2;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1000);
        upgradeCost = 1000;
        DisableEquippedButton(data);
    }

    public void TripleMissileSelect()
    {
        toggles[6] = toggles[6] ? false : true;
        SetAllSelectToFalse(6);
        if (toggles[6])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[2];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupselect = -3;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(1500);
        upgradeCost = 1500;
        DisableEquippedButton(data);
    }

    public void SwarmerMissileSelect()
    {
        toggles[7] = toggles[7] ? false : true;
        SetAllSelectToFalse(7);
        if (toggles[7])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Swarmer Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 8 swarmer missiles at a time. Swarmer missiles do half damage of regular missiles";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[3];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = -4;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void SingleEscortSelect()
    {
        toggles[8] = toggles[8] ? false : true;
        SetAllSelectToFalse(8);
        if (toggles[8])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Single Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "1 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = escortSprites[0];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupselect = 9001;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void DoubleEscortSelect()
    {
        toggles[9] = toggles[9] ? false : true;
        SetAllSelectToFalse(9);
        if (toggles[9])
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
        upgradeCost = 1000;
        DisableEquippedButton(data);
    }

    public void TripleEscortSelect()
    {
        toggles[10] = toggles[10] ? false : true;
        SetAllSelectToFalse(10);
        if (toggles[10])
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
        upgradeCost = 1500;
        DisableEquippedButton(data);
    }

    public void QuadEscortSelect()
    {
        toggles[11] = toggles[11] ? false : true;
        SetAllSelectToFalse(11);
        if (toggles[11])
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
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void AutoCannonSelect()
    {
        toggles[12] = toggles[12] ? false : true;
        SetAllSelectToFalse(12);
        if (toggles[12])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Auto Cannon";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 5 bullets per second";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[4];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = 4;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void HighVelocityShotSelect()
    {
        toggles[13] = toggles[13] ? false : true;
        SetAllSelectToFalse(13);
        if (toggles[13])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "High Velocity Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 1 high velocity bullet that does TONS OF DAMAGE";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[5];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = 5;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void UpgradeDoubleShotSelect()
    {
        toggles[14] = toggles[14] ? false : true;
        SetAllSelectToFalse(14);
        if (toggles[14])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Upgraded Double Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 2 bullets at a time with increased damage";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[6];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupselect = 6;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(3000);
        upgradeCost = 3000;
        DisableEquippedButton(data);
    }

    public void DoubleAutoCannonSelect()
    {
        toggles[15] = toggles[15] ? false : true;
        SetAllSelectToFalse(15);
        if (toggles[15])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Auto Cannon";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 10 bullets per second";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[7];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupselect = 7;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(3000);
        upgradeCost = 3000;
        DisableEquippedButton(data);
    }

    public void SmartHighVelocityShotSelect()
    {
        toggles[16] = toggles[16] ? false : true;
        SetAllSelectToFalse(16);
        if (toggles[16])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Smart High Velocity Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 1 high velocity bullet that does TONS OF DAMAGE. Bullet moves towards mouse direction upon firing";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = gunSprites[8];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupselect = 8;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(3000);
        upgradeCost = 3000;
        DisableEquippedButton(data);
    }

    public void PoisonMissileSelect()
    {
        toggles[17] = toggles[17] ? false : true;
        SetAllSelectToFalse(17);
        if (toggles[17])
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Poison Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires a poison missile. Poison missiles slows all enemies in an area and does damage over time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = missileSprites[4];
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupselect = -5;
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }
    #endregion
}
