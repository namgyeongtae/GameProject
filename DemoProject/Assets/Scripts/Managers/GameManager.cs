using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private Managers _managers = new Managers();

    private void Start()
    {
        Init();
        _managers.Init();
    }

    private void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);

            _instance = go.GetOrAddComponent<GameManager>();
        }
    }
}
