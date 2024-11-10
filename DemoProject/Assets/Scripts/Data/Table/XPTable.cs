using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public float hp;
    public int exp_required;
    public int attack;
    public int defense;
    public float speed;
}

[System.Serializable]
public class LevelingDataWrapper
{
    public LevelData[] playerStats;
}

public class XPTable
{
    public Dictionary<int, LevelData> XP_Dict = new Dictionary<int, LevelData>();

    public void Init()
    {
        LoadTable();
    }

    private void LoadTable()
    {
        // TODO
        // 경로 바꿔야 함
        string path = Path.Combine(Application.streamingAssetsPath, "XPTable.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            LevelingDataWrapper dataWrapper = JsonUtility.FromJson<LevelingDataWrapper>(json);

            foreach (var data in dataWrapper.playerStats)
            {
                XP_Dict[data.level] = data;
            }
        }
        else
        {
            Debug.LogError("Experience table JSON file not found.");
        }
    }

    public LevelData GetStatForLevel(int level)
    {
        if (XP_Dict.ContainsKey(level))
        {
            return XP_Dict[level];
        }

        return null;
    }
}
