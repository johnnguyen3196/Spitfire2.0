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

    private bool tripleshot = false;
    private bool quadshot = false;
    private bool upgradetripleshot = false;

    private bool doublemissile = false;
    private bool triplemissile = false;
    private bool quadmissile = false;

    private bool doubleescort = false;
    private bool tripleescort = false;
    private bool quadescirt = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("saveName"));
        InitializeGunUpgrades(data.shootType);
        InitializeMissileUpgrades(data.missileType);
        InitializeEscortUpgrades(data.escortType - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void TripleShotSelect()
    {
        tripleshot = tripleshot ? false : true;
        if (tripleshot)
        {
            Info.SetActive(true);
            Info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Triple Shot";
            Info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Fire 3 bullets at a time";
            Info.transform.GetChild(3).GetComponent<Image>().sprite = sprites[0]; 
        } else
        {
            Info.SetActive(false);
        }
    }
}
