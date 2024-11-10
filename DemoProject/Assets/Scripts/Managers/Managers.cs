using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    private static InputManager _input = new InputManager();
    private static CharacterManager _character = new CharacterManager();
    private static DataManager _data = new DataManager();
    private static ResourceManager _resource = new ResourceManager();
    private static PoolManager _pool = new PoolManager();
    private static UIManager _ui = new UIManager();
    private static XPManager _xp = new XPManager();
    private static CameraManager _camera = new CameraManager();
    private static TilemapManager _tileMap = new TilemapManager();

    public static InputManager Input { get { return _input; } }
    public static CharacterManager Character { get { return _character; } }
    public static DataManager Data { get { return _data; } }
    public static ResourceManager Resource { get { return _resource; } }
    public static PoolManager Pool { get { return _pool; } }
    public static UIManager UI { get { return _ui; } }
    public static XPManager XP { get { return _xp; } }
    public static CameraManager Camera { get { return _camera;} }
    public static TilemapManager TileMap { get { return _tileMap; } }

    public void Init()
    {
        _input.Init();
        _pool.Init();
        _tileMap.Init();
        _data.Init();
        _character.Init();
        _camera.Init();
        _xp.Init();
    }

    public void OnUpdate()
    {
        _input.Update();
    }

    public void Clear()
    {

    }
}
