using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Manager
{
    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    private Transform _root;

    public override void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (!_pool.ContainsKey(name))
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!_pool.ContainsKey(original.name))
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!_pool.ContainsKey(name))
            return null;
        return _pool[name].Original;
    }

    public override void Clear()
    {
        foreach (Transform child in _root)
            Managers.Resource.Destroy(child.gameObject);

        _pool.Clear();
    }
}
