using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public string saveName;
    public int plane;

    public PlayerData(Player player)
    {
        level = player.level;
        saveName = player.saveName;
        plane = player.plane;
    }
}
