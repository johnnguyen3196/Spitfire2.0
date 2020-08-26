using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public string saveName;
    public int plane;

    public int shootType;
    public int missileType;
    public string escortType;

    public List<string> researchedGunUpgrades;
    public List<string> researchedMissileUpgrades;
    public List<string> researchedEscortUpgrades;

    public int points;

    public int slot;

    public PlayerData(Player player)
    {
        level = player.level;
        saveName = player.saveName;
        plane = player.plane;

        shootType = player.shootType;
        missileType = player.missileType;
        escortType = player.escortType;
    }

    public PlayerData()
    {

    }
}
