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
    public int escortType;

    public int points = 1000;

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
}
