using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    private static InputManager _input = new InputManager();
    private static ResourceManager _resource = new ResourceManager();
    private static PoolManager _pool = new PoolManager();
    private static UIManager _ui = new UIManager();
    private static CameraManager _camera = new CameraManager();
    private static TilemapManager _tileMap = new TilemapManager();

    public static InputManager Input { get { return _input; } }
    public static ResourceManager Resource { get { return _resource; } }
    public static PoolManager Pool { get { return _pool; } }
    public static UIManager UI { get { return _ui; } }
    public static CameraManager Camera { get { return _camera;} }
    public static TilemapManager TileMap { get { return _tileMap; } }

    public void Init()
    {
        _input.Init();
        _pool.Init();
        _tileMap.Init();
    }

    public void Clear()
    {

    }
}
