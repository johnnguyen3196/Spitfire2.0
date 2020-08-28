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

    public Sprite EmptySprite;

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

    private PlayerAttack weaponSelect;
    private string powerupName;

    private bool escortSelected = false;

    private int upgradeCost;

    [System.Serializable]
    public struct Node {
        public string name;
        public int powerup;
        public string[] Children;
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

        data.points = 10000;

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
        PlayerAttack gunSelect = FindObjectOfType<WeaponManager>().FindGun(data.shootType);
        PlayerAttack missileSelect = FindObjectOfType<WeaponManager>().FindMissile(data.missileType);
        EquippedObject.transform.GetChild(0).GetComponent<Image>().sprite = gunSelect.GetSprite();
        EquippedObject.transform.GetChild(1).GetComponent<Image>().sprite = missileSelect.GetSprite();
        EquippedObject.transform.GetChild(2).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find(data.escortType);
    }
    #endregion

    #region Initialize all tree upgrades
    void InitializeGunUpgrades(List<string> upgrades)
    {
        foreach (string upgrade in upgrades)
        {
            int index = Array.FindIndex(GunTree, node => node.name == upgrade);
            if (index == -1)
            {
                Debug.LogWarning(upgrade + " not found in GunTree");
                return;
            }
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = GunTreeObject.transform.Find(upgrade).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach(string child in GunTree[index].Children)
            {
                GameObject childToggleObject = GunTreeObject.transform.Find(child).gameObject;
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

    void InitializeMissileUpgrades(List<string> upgrades)
    {
        foreach (string upgrade in upgrades)
        {
            int index = Array.FindIndex(MissileTree, node => node.name == upgrade);
            if(index == -1)
            {
                Debug.LogWarning(upgrade + " not found in MissileTree");
                return;
            }
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = MissileTreeObject.transform.Find(upgrade).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach (string child in MissileTree[index].Children)
            {
                GameObject childToggleObject = MissileTreeObject.transform.Find(child).gameObject;
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

    void InitializeEscortUpgrades(List<string> upgrades)
    {
        foreach (string upgrade in upgrades)
        {
            int index = Array.FindIndex(EscortTree, node => node.name == upgrade);
            if (index == -1)
            {
                Debug.LogWarning(upgrade + " not found in EscortTree");
                return;
            }
            //enable toggle for upgrades the player researched 
            GameObject toggleObject = EscortTreeObject.transform.Find(upgrade).gameObject;
            Toggle toggle = toggleObject.GetComponent<Toggle>();
            toggle.interactable = true;

            //enable toggle for children of current upgrade
            foreach (string child in EscortTree[index].Children)
            {
                GameObject childToggleObject = EscortTreeObject.transform.Find(child).gameObject;
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
        if (escortSelected)
        {
            data.escortType = powerupName;
            data.researchedEscortUpgrades.Add(powerupName);
            InitializeEscortUpgrades(data.researchedEscortUpgrades);
            escortSelected = false;
        } else
        {
            if (weaponSelect.type == PlayerAttack.Type.Gun)
            {
                data.shootType = weaponSelect.id;
                data.researchedGunUpgrades.Add(powerupName);
                InitializeGunUpgrades(data.researchedGunUpgrades);
            }
            else
            {
                data.missileType = weaponSelect.id;
                data.researchedMissileUpgrades.Add(powerupName);
                InitializeMissileUpgrades(data.researchedMissileUpgrades);
            }
        }
        data.points -= upgradeCost;
        SetPointsText(data.points);
        SetEquippedImage(data);
    }

    public void Equip()
    {
        Info.SetActive(false);
        if (escortSelected)
        {
            data.escortType = powerupName; 
            SetEquippedImage(data);
            escortSelected = false;
            return;
        }
        //Figure out which powerup was selected, modify the save data, let the next upgrade be available
        if (weaponSelect.type == PlayerAttack.Type.Gun)
        {
            data.shootType = weaponSelect.id;
        }
        else
        {
            data.missileType = weaponSelect.id;
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
        bool checkEscort = false;
        bool checkGun = false;
        bool checkMissile = false;

        if (escortSelected && data.researchedEscortUpgrades.FindIndex(upgrade => upgrade == powerupName) != -1)
        {
            checkEscort = true;
        }
        //edge case
        if(weaponSelect == null && escortSelected)
        {
            return checkEscort;
        }

        if (weaponSelect.type == PlayerAttack.Type.Gun && data.researchedGunUpgrades.FindIndex(upgrade => upgrade == powerupName) != -1)
        {
            checkGun = true;
        }

        if (weaponSelect.type == PlayerAttack.Type.Missile && data.researchedMissileUpgrades.FindIndex(upgrade => upgrade == powerupName) != -1)
        {
            checkMissile = true;
        }
        return checkEscort || checkGun || checkMissile;
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
            weaponSelect = new DoubleShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 2 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "DoubleShot";
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
            weaponSelect = new TripleShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupName = "TripleShot";
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
            weaponSelect = new QuadShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quad Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 4 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupName = "QuadShot";
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
            weaponSelect = new UpgradedTripleShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Upgrade Triple Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 bullets at a time. Center bullet does increased damage.";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "UpgradeTripleShot";
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
            weaponSelect = new SingleMissile();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Single Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 1 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "SingleMissile";
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
            weaponSelect = new DoubleMissile();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 2 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupName = "DoubleMissile";
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
            weaponSelect = new TripleMissile();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 missiles at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupName = "TripleMissile";
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
            weaponSelect = new SwarmerMissile();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Swarmer Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 8 swarmer missiles at a time. Swarmer missiles do half damage of regular missiles";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "SwarmerMissile";
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
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Single Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "1 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Squadron1");
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "Squadron1";
        }
        else
        {
            escortSelected = false;
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
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "2 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Squadron2");
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1000";
            powerupName = "Squadron2";
        }
        else
        {
            escortSelected = false;
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
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "3 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Squadron3");
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 1500";
            powerupName = "Squadron3";
        }
        else
        {
            escortSelected = false;
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
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quad Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "4 Escorts wil rotate around player";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Squadron4");
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "Squadron4";
        }
        else
        {
            escortSelected = false;
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
            weaponSelect = new AutoCannon();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Auto Cannon";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 5 bullets per second";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "AutoCannon";
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
            weaponSelect = new HighVelocityShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "High Velocity Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 1 high velocity bullet that does TONS OF DAMAGE";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "HighVelocityShot";
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
            weaponSelect = new UpgradedDoubleShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Upgraded Double Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 2 bullets at a time with increased damage";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupName = "UpgradeDoubleShot";
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
            weaponSelect = new DoubleAutoCannon();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Double Auto Cannon";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 10 bullets per second";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupName = "DoubleAutoCannon";
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
            weaponSelect = new SmartHighVelocityShot();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Smart High Velocity Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires 1 high velocity bullet that does TONS OF DAMAGE. Bullet moves towards mouse direction upon firing";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 3000";
            powerupName = "SmartHighVelocityShot";
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
            weaponSelect = new SinglePoisonMissile();
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Poison Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fires a poison missile. Poison missiles slows all enemies in an area and does damage over time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = weaponSelect.GetSprite();
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "PoisonMissile";
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void DisableGunSelect()
    {
        toggles[18] = toggles[18] ? false : true;
        SetAllSelectToFalse(18);
        if (toggles[18])
        {
            weaponSelect = new PlayerAttack();
            weaponSelect.type = PlayerAttack.Type.Gun;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Disable Guns";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Your guns will not shoot";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = EmptySprite;
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "DisableGun";
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void DisableMissileSelect()
    {
        toggles[19] = toggles[19] ? false : true;
        SetAllSelectToFalse(19);
        if (toggles[19])
        {
            weaponSelect = new PlayerAttack();
            weaponSelect.type = PlayerAttack.Type.Missile;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Disable Missile";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Your missiles will not shoot";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = EmptySprite;
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "DisableMissile";
        }
        else
        {
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void DisableEscortSelect()
    {
        toggles[20] = toggles[20] ? false : true;
        SetAllSelectToFalse(20);
        if (toggles[20])
        {
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Disable Escort";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "No escorts will fly around you";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = EmptySprite;
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 0";
            powerupName = "DisableEscort";
        }
        else
        {
            escortSelected = false;
            Info.SetActive(false);
        }
        DisableUpgradeButton(0);
        upgradeCost = 0;
        DisableEquippedButton(data);
    }

    public void SingleSwordSelect()
    {
        toggles[21] = toggles[21] ? false : true;
        SetAllSelectToFalse(21);
        if (toggles[21])
        {
            escortSelected = true;
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sword";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "A sword will rotate and deal damage to enemies on-hit. The sword also has a 50% chance to destroy enemy projectiles.";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Sword1");
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            powerupName = "Sword1";
        }
        else
        {
            escortSelected = false;
            Info.SetActive(false);
        }
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }

    public void SingleLancasterSelect()
    {
        // Make sure that the integer in the next 3 lines are not used in other functions and are the same in this function. Failure to do this will cause weird Toggle issues
        toggles[22] = toggles[22] ? false : true;
        SetAllSelectToFalse(22);
        if (toggles[22])
        {
            //For upgrading escorts, set this variable to true
            escortSelected = true;
            Info.SetActive(true);
            // Set the name of your Escort upgrade
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lancaster";
            // Set the description of your Escort upgrade
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "A bomber escort will rotate around the player and shoot bombs. The bomb will explode if the player's mouse is below the bomb projectile.";
            // Find your UI Sprite from the SpriteManager by name
            Info.transform.GetChild(3).GetComponent<Image>().sprite = FindObjectOfType<UISpriteManager>().Find("Lancaster1");
            // Set the cost text of your Escort upgrade
            Info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Cost: 2000";
            // Set the Name of your Escort upgrade. **Important** make sure it is the same as your Toggle name
            powerupName = "Lancaster1";
        }
        else
        {
            escortSelected = false;
            Info.SetActive(false);
        }
        //The next two numbers will be the cost of your upgrade in points
        DisableUpgradeButton(2000);
        upgradeCost = 2000;
        DisableEquippedButton(data);
    }
    #endregion
}
