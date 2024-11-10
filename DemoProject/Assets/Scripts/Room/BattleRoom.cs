using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : Room
{
    [SerializeField] private SerializableDictionary<Transform, GameObject> _spawnPosMob;

    // Start is called before the first frame update
    void Start()
    {
        BindEvents();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void BindEvents()
    {
        EnterRoom -= StartBattle;
        EnterRoom += StartBattle;
    }

    private void StartBattle()
    {
        foreach (var key in _spawnPosMob.Keys)
        {
            var go = Managers.Resource.Instantiate(_spawnPosMob[key]);
            go.transform.position = key.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            EnterRoom.Invoke();
    }
}
