using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Manager
{
    private Tilemap _floor;
    private Tilemap _walls;
    private Tilemap _deco;

    public Dictionary<TileLayer, Tilemap> WorldTilemap = new Dictionary<TileLayer, Tilemap>();

    public override void Init()
    {
        _floor = GameObject.Find("Floor").GetComponent<Tilemap>();
        _walls = GameObject.Find("Walls").GetComponent<Tilemap>();
        _deco = GameObject.Find("Deco").GetComponent<Tilemap>();

        WorldTilemap.Add(TileLayer.Floor, _floor);
        WorldTilemap.Add(TileLayer.Wall, _walls);
        WorldTilemap.Add(TileLayer.Deco, _deco);
    }

    public override void Clear()
    {
        _floor = null;
        _walls = null;
        _deco = null;
    }

    public void OnLoadScene_Event()
    {

    }
}
