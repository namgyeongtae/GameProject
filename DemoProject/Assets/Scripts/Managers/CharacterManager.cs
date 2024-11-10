using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : Manager
{
    public Player Player;
    public UserData UserData;

    public override void Init()
    {
        if (UserData == null)
            LoadUserData();

        if (Player == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                player = Managers.Resource.Instantiate("Character/Player");
            }

            Player = player.GetComponent<Player>();
            Player.transform.position = new Vector3(5f, -6f, 0f);
            Player.Init();
        }
    }

    private void LoadUserData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"JSON file not found at path : {filePath}");
            return;
        }

        string jsonContent = File.ReadAllText(filePath);

        UserData = JsonUtility.FromJson<UserData>(jsonContent);
    }
}
