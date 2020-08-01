using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static PlayerData SavePlayer(Player player, string saveName, int slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".save";
        UnityEngine.Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        if(slot != -1)
        {
            data.slot = slot;
        }

        formatter.Serialize(stream, data);
        stream.Close();

        return data;
    }

    public static PlayerData LoadPlayer(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
