using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerStat
{
    public PlayerStat()
    {
        
    }

    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(Managers.Character.UserData, true);

        string filePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        File.WriteAllText(filePath, json);
    }
}
